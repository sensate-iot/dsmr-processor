using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Logic;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public sealed class ProcessingService : IProcessingService
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(ProcessingService));
		private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);

		private IList<SensorMapping> m_sensors;
		private readonly ISensorMappingRepository m_sensorMappings;
		private readonly IProcessingHistoryRepository m_history;
		private readonly object m_lock;
		private readonly IDataClient m_client;
		private readonly ISystemClock m_clock;


		public ProcessingService(ISensorMappingRepository repo,
		                         IProcessingHistoryRepository history,
		                         IDataClient client,
		                         ISystemClock clock)
		{
			this.m_sensorMappings = repo;
			this.m_history = history;
			this.m_client = client;
			this.m_clock = clock;
			this.m_lock = new object();
		}

		public async Task LoadSensorMappingsAsync(CancellationToken ct)
		{
			var rawMappings = await this.m_sensorMappings.GetAllSensorsAsync(ct).ConfigureAwait(false);
			var mappings = rawMappings.ToList();

			foreach(var sensorMapping in mappings) {
				var last = await this.m_history.GetLastProcessingTimestamp(sensorMapping.PowerSensorId).ConfigureAwait(false);
				sensorMapping.LastProcessed = last.End;
			}

			lock(this.m_lock) {
				this.m_sensors = mappings.ToList();
			}
		}

		public void Process(CancellationToken ct)
		{
			logger.Info("Starting processing of sensors.");

			lock(this.m_lock) {
				if(this.m_sensors == null) {
					return;
				}

				foreach(var sensor in this.m_sensors) {
					this.processAsync(sensor, ct).GetAwaiter().GetResult();
				}
			}

			logger.Info("Finished processing cycle.");
		}

		private async Task processAsync(SensorMapping mapping, CancellationToken ct)
		{
			var now = this.m_clock.GetCurrentTime();
			var threshold = mapping.LastProcessed.Add(Interval);
			var end = roundDown(now, Interval);

			if(threshold > now) {
				return;
			}

			var pwrData = await this.m_client.GetRange(mapping.PowerSensorId, mapping.LastProcessed, end, ct)
				.ConfigureAwait(false);
			var resultSet = DataCalculator.ComputePowerAverages(pwrData);


			if(mapping.EnvironmentSensorId != null) {
				var envTask = this.m_client.GetRange(mapping.EnvironmentSensorId, mapping.LastProcessed, end, ct);
				var envData = await envTask.ConfigureAwait(false);
				DataCalculator.ComputeEnvironmentAverages(resultSet, envData);
			}

			if(mapping.GasSensorId != null) {
				var gasTask = this.m_client.GetRange(mapping.GasSensorId, mapping.LastProcessed, end, ct);
				var gasData = await gasTask.ConfigureAwait(false);
				DataCalculator.ComputeGasAverages(resultSet, gasData);
			}

		}

		private static DateTime roundDown(DateTime dt, TimeSpan span)
		{
			var delta = dt.Ticks % span.Ticks;
			return new DateTime(dt.Ticks - delta, dt.Kind);
		}

		public void Dispose()
		{
			this.m_history.Dispose();
			this.m_sensorMappings.Dispose();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Logic;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

using SensorMapping = SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.SensorMapping;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public sealed class ProcessingService : IProcessingService
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(ProcessingService));
		private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);

		private IList<SensorMapping> m_sensors;
		private readonly ISensorMappingRepository m_sensorMappings;
		private readonly IProcessingHistoryRepository m_history;
		private readonly IDataPointRepository m_dataRepo;
		private readonly object m_lock;
		private readonly IDataClient m_client;
		private readonly ISystemClock m_clock;
		private readonly IWeatherService m_openWeather;
		private readonly string m_serviceName;

		public ProcessingService(string serviceName,
		                         ISensorMappingRepository repo,
		                         IDataPointRepository dataRepo,
		                         IProcessingHistoryRepository history,
		                         IDataClient client,
		                         ISystemClock clock,
		                         IWeatherService service)
		{
			this.m_sensorMappings = repo;
			this.m_history = history;
			this.m_dataRepo = dataRepo;
			this.m_client = client;
			this.m_clock = clock;
			this.m_openWeather = service;
			this.m_lock = new object();
			this.m_serviceName = serviceName;
		}

		public async Task LoadSensorMappingsAsync(CancellationToken ct)
		{
			var rawMappings = await this.m_sensorMappings.GetAllSensorsAsync(this.m_serviceName, ct).ConfigureAwait(false);
			var mappings = rawMappings.ToList();

			foreach(var sensorMapping in mappings) {
				var last = await this.m_history.GetLastProcessingTimestamp(sensorMapping.Id).ConfigureAwait(false);
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

			logger.Info($"Processing {mapping.Id}.");

			if(threshold > now) {
				logger.Info("Stopped processing. Timestamp threshold past current timestamp.");
				return;
			}

			var resultSet = await this.computeAggregates(mapping, end, ct).ConfigureAwait(false);

			if(resultSet == null) {
				return;
			}

			logger.Debug("Finished calculating averages. Starting storage process.");
			await this.storeDataPoints(resultSet, ct).ConfigureAwait(false);
			await this.m_history.CreateProcessingTimestamp(mapping.Id, resultSet.Count, mapping.LastProcessed, end, ct).ConfigureAwait(false);

			await this.cleanupOldRequests(mapping, end, ct).ConfigureAwait(false);
			mapping.LastProcessed = end;

			logger.Info($"Finished processing {mapping.Id}.");
		}

		private async Task cleanupOldRequests(SensorMapping mapping, DateTime lastProcessingTime, CancellationToken ct)
		{
			if(mapping.LastProcessed.Hour != lastProcessingTime.Hour) {
				await this.m_client.DeleteBucketsAsync(mapping.PowerSensorId, DateTime.MinValue, lastProcessingTime, ct)
					.ConfigureAwait(false);

				if(!string.IsNullOrEmpty(mapping.GasSensorId)) {
					await this.m_client
						.DeleteBucketsAsync(mapping.GasSensorId, DateTime.MinValue, lastProcessingTime, ct)
						.ConfigureAwait(false);
				}

				if(!string.IsNullOrEmpty(mapping.EnvironmentSensorId)) {
					await this.m_client
						.DeleteBucketsAsync(mapping.EnvironmentSensorId, DateTime.MinValue, lastProcessingTime, ct)
						.ConfigureAwait(false);
				}
			}
		}

		private async Task<IDictionary<DateTime, DataPoint>> computeAggregates(SensorMapping mapping, DateTime end, CancellationToken ct)
		{
			var resultSet = await this.processPowerData(mapping, end, ct).ConfigureAwait(false);

			if(resultSet == null) {
				logger.Warn("Stopped processing. No E-data received.");
				return null;
			}

			await this.processEnvData(mapping, resultSet, end, ct).ConfigureAwait(false);
			await this.processGasData(mapping, resultSet, end, ct).ConfigureAwait(false);

			return resultSet;
		}

		private async Task<IDictionary<DateTime, DataPoint>> processPowerData(SensorMapping mapping, DateTime end, CancellationToken ct)
		{
			var rawPowerData = await this.m_client.GetRangeAsync(mapping.PowerSensorId, mapping.LastProcessed, end, ct)
				.ConfigureAwait(false);
			var pwrData = rawPowerData?.ToList();

			if(pwrData == null || pwrData.Count <= 0) {
				return null;
			}

			var resultSet = DataCalculator.ComputePowerAverages(mapping, pwrData);

			var span = pwrData[0].Timestamp - this.m_clock.GetCurrentTime();

			if(span > TimeSpan.FromHours(3)) {
				return resultSet;
			}

			// Get the longitude and latitude. Assume these are all the same
			// for other measurements (.. a smart meter is quite unlikely
			// to move).
			var first = pwrData[0].Location;
			await this.lookupWeather(mapping, resultSet, first, ct).ConfigureAwait(false);

			return resultSet;
		}

		private async Task lookupWeather(SensorMapping mapping, IDictionary<DateTime, DataPoint> resultSet, Location location, CancellationToken ct)
		{
			var lookup = new WeatherLookup {
				Longitude = location.Longitude,
				Latitude = location.Latitude,
				SensorId = mapping.Id
			};

			var result = await this.m_openWeather.LookupAsync(lookup, ct).ConfigureAwait(false);
			decimal? oat = null;

			if(result != null) {
				oat = Convert.ToDecimal(result.Temperature);
			}

			foreach(var keyValuePair in resultSet) {
				keyValuePair.Value.OutsideAirTemperature = oat;
			}
		}

		private async Task processEnvData(SensorMapping mapping, IDictionary<DateTime, DataPoint> data, DateTime end, CancellationToken ct)
		{
			if(mapping.EnvironmentSensorId != null) {
				logger.Info("Processing environment data.");
				var envTask = this.m_client.GetRangeAsync(mapping.EnvironmentSensorId, mapping.LastProcessed, end, ct);
				var envData = await envTask.ConfigureAwait(false);
				DataCalculator.ComputeEnvironmentAverages(data, envData);
				logger.Debug("Finished processing environment data.");
			}
		}

		private async Task processGasData(SensorMapping mapping, IDictionary<DateTime, DataPoint> data, DateTime end, CancellationToken ct)
		{
			if(mapping.GasSensorId != null) {
				logger.Info("Processing gas data.");
				var gasTask = this.m_client.GetRangeAsync(mapping.GasSensorId, mapping.LastProcessed, end, ct);
				var gasData = await gasTask.ConfigureAwait(false);
				DataCalculator.ComputeGasAverages(data, gasData);
				logger.Debug("Finished processing gas data.");
			}
		}

		private async Task storeDataPoints(IDictionary<DateTime, DataPoint> measurements, CancellationToken ct)
		{
			var datapoints = measurements.Select(x => x.Value);
			await this.m_dataRepo.CreateBulkDataPointsAsync(datapoints, ct).ConfigureAwait(false);
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

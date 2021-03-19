using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public sealed class ProcessingService : IProcessingService
	{
		private readonly ISensorMappingRepository m_sensorMappings;
		private IList<SensorMapping> m_sensors;
		private readonly IProcessingHistoryRepository m_history;
		private readonly object m_lock;

		public ProcessingService(ISensorMappingRepository repo, IProcessingHistoryRepository history)
		{
			this.m_sensorMappings = repo;
			this.m_history = history;
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

		public async Task ProcessAsync(CancellationToken ct)
		{
			IEnumerable<string> sensorIds;

			lock(this.m_lock) {
				sensorIds = this.m_sensors.Select(x => x.PowerSensorId);
			}
		}

		public void Dispose()
		{
			this.m_sensorMappings.Dispose();
		}
	}
}

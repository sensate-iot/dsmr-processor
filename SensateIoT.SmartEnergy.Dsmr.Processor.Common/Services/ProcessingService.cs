using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public sealed class ProcessingService : IProcessingService
	{
		private readonly ISensorMappingRepository m_sensorMappings;
		private IList<SensorMapping> m_sensors;
		private readonly object m_lock;

		public ProcessingService(ISensorMappingRepository repo)
		{
			this.m_sensorMappings = repo;
			this.m_lock = new object();
		}

		public async Task LoadSensorMappingsAsync(CancellationToken ct)
		{
			var mappings = await this.m_sensorMappings.GetAllSensorsAsync(ct).ConfigureAwait(false);

			lock(this.m_lock) {
				this.m_sensors = mappings.ToList();
			}
		}

		public async Task ProcessAsync(CancellationToken ct)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			this.m_sensorMappings.Dispose();
		}
	}
}

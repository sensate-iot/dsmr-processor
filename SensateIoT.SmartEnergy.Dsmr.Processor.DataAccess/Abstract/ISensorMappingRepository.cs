using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract
{
	public interface ISensorMappingRepository : IDisposable
	{
		Task<SensorMapping> GetSensorMapping(string sensorId, CancellationToken ct = default);
		Task<IEnumerable<SensorMapping>> GetAllSensorsAsync(CancellationToken ct = default);
	}
}

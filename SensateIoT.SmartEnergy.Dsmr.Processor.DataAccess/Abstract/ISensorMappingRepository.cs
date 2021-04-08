using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract
{
	public interface ISensorMappingRepository : IDisposable
	{
		Task<IEnumerable<SensorMapping>> GetAllSensorsAsync(string serviceName, CancellationToken ct = default);
	}
}

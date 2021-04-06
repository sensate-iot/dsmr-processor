using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract
{
	public interface IDataClient : IDisposable
	{
		Task<IEnumerable<Measurement>> GetRangeAsync(string sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task DeleteBucketsAsync(string sensorId, DateTime start, DateTime end, CancellationToken ct);
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract
{
	public interface IProcessingHistoryRepository : IDisposable
	{
		Task<ProcessingTimestamp> GetLastProcessingTimestamp(int sensorId);
		Task CreateProcessingTimestamp(int sensorId, int count, DateTime start, DateTime end, CancellationToken ct);
	}
}
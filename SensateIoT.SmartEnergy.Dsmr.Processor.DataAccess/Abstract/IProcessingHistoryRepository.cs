using System;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract
{
	public interface IProcessingHistoryRepository : IDisposable
	{
		Task<ProcessingTimestamp> GetLastProcessingTimestamp(string sensorId);
	}
}
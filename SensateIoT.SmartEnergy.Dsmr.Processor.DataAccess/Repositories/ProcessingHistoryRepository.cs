using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories
{
	public class ProcessingHistoryRepository : AbstractRepository, IProcessingHistoryRepository
	{
		private const string DsmrProcessor_SelectLastProcessedBySensorId = "DsmrProcessor_SelectLastProcessedBySensorId";

		public ProcessingHistoryRepository(string connection) : base(new SqlConnection(connection))
		{
		}

		public async Task<ProcessingTimestamp> GetLastProcessingTimestamp(string sensorId)
		{
			var result = await this.QuerySingleAsync<ProcessingTimestamp>(DsmrProcessor_SelectLastProcessedBySensorId,
				"@sensorId", sensorId).ConfigureAwait(false);

			if(result == null) {
				result = new ProcessingTimestamp {
					End = DateTime.MinValue,
					SensorId = sensorId,
					Start = DateTime.MinValue
				};
			}

			return result;
		}
	}
}

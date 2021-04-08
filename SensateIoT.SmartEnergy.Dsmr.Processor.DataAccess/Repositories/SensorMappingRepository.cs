using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories
{
	public class SensorMappingRepository : AbstractRepository, ISensorMappingRepository
	{
		private const string SelectAllSensorMappings = "DsmrProcessor_GetDevices";

		public SensorMappingRepository(string connectionString) : base(new SqlConnection(connectionString))
		{
		}

		public async Task<IEnumerable<SensorMapping>> GetAllSensorsAsync(string serviceName, CancellationToken ct = default)
		{
			var result = await this.QueryAsync<Data.Models.SensorMapping>(SelectAllSensorMappings, "@processorServiceName", serviceName)
				.ConfigureAwait(false);

			return result.Select(x => new SensorMapping {
				EnvironmentSensorId = x.EnvironmentSensorId,
				GasSensorId = x.GasSensorId,
				Id = x.Id,
				PowerSensorId = x.PowerSensorId,
				LastProcessed = default
			});
		}
	}
}

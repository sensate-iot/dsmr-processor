using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories
{
	public class SensorMappingRepository : AbstractRepository, ISensorMappingRepository
	{
		private const string SelectSensorMappings = "DsmrProcessor_SelectSensorMapping";
		private const string SelectAllSensorMappings = "DsmrProcessor_SelectAllSensorMappings";

		public SensorMappingRepository(string connectionString) : base(new SqlConnection(connectionString))
		{
		}

		public Task<SensorMapping> GetSensorMapping(string sensorId, CancellationToken ct)
		{
			return this.QuerySingleAsync<SensorMapping>(SelectSensorMappings, "@sensorId", sensorId);
		}

		public async Task<IEnumerable<SensorMapping>> GetAllSensorsAsync(CancellationToken ct = default)
		{
			return await this.QueryAsync<SensorMapping>(SelectAllSensorMappings).ConfigureAwait(false);
		}
	}
}

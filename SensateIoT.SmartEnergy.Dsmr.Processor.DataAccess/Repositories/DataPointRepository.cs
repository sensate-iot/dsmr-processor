using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories
{
	public class DataPointRepository : AbstractRepository, IDataPointRepository
	{
		private const string InsertDataPoint = "DsmrProcessor_InsertDataPoint";

		public DataPointRepository(string connectionString) : base(new SqlConnection(connectionString))
		{
		}

		public async Task CreateDataPointAsync(DataPoint dp, CancellationToken ct = default)
		{
			await this.ExecuteAsync(
				InsertDataPoint,
				"@sensorId", dp.SensorId,
				"@powerUsage", dp.PowerUsage,
				"@powerProduction", dp.PowerProduction,
				"@energyUsage", dp.EnergyUsage,
				"@energyProduction", dp.PowerProduction,
				"@gasUsage", dp.GasUsage,
				"@gasFlow", dp.GasFlow,
				"@oat", dp.OutsideAirTemperature,
				"@date", dp.Timestamp
			).ConfigureAwait(false);
		}
	}
}

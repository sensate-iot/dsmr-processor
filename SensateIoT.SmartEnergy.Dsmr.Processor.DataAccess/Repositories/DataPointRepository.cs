using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories
{
	public class DataPointRepository : AbstractRepository, IDataPointRepository
	{
		private const string InsertDataPoint = "DsmrProcessor_InsertDataPoint";
		private readonly string m_connectionString;

		public DataPointRepository(string connectionString) : base(new SqlConnection(connectionString))
		{
			this.m_connectionString = connectionString;
		}

		public async Task CreateDataPointAsync(DataPoint dp, CancellationToken ct = default)
		{
			await this.ExecuteAsync(
				InsertDataPoint,
				"@sensorId", dp.SensorId,
				"@powerUsage", dp.PowerUsage,
				"@powerProduction", dp.PowerProduction,
				"@energyUsage", dp.EnergyUsage,
				"@energyProduction", dp.EnergyProduction,
				"@tariff", dp.Tariff,
				"@gasUsage", dp.GasUsage,
				"@gasFlow", dp.GasFlow,
				"@oat", dp.OutsideAirTemperature,
				"@temperature", dp.Temperature,
				"@pressure", dp.Pressure,
				"@rh", dp.RelativeHumidity,
				"@date", dp.Timestamp
			).ConfigureAwait(false);
		}

		public async Task CreateBulkDataPointsAsync(IEnumerable<DataPoint> dataPoints, CancellationToken ct = default)
		{
			var dt = new DataTable();

			buildColumns(dt);

			foreach(var dp in dataPoints) {
				addRow(dt, dp);
			}

			await this.executeBulkQuery(dt, ct).ConfigureAwait(false);
		}

		private async Task executeBulkQuery(DataTable table, CancellationToken ct)
		{
			var param = new SqlParameter {
				ParameterName = "@data",
				SqlDbType = SqlDbType.Structured,
				Value = table
			};

			using(var conn = new SqlConnection(this.m_connectionString)) {
				await conn.OpenAsync(ct).ConfigureAwait(false);

				var cmd = new SqlCommand("[dbo].[DsmrProcessor_BulkInsertDataPoints]") {
					Connection = conn, CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.Add(param);
				await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
			}
		}

		private static void addRow(DataTable table, DataPoint dp)
		{
			var row = table.NewRow();
			var props = dp.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			for(var idx = 0; idx < props.Length; idx++) {
				var prop = props[idx];
				row[idx] = prop.GetValue(dp) ?? DBNull.Value;
			}

			table.Rows.Add(row);
		}

		private static void buildColumns(DataTable dt)
		{
			var props = typeof(DataPoint).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach(var propertyInfo in props) {
				var col = new DataColumn {
					ColumnName = propertyInfo.Name,
					DataType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType,
					AllowDBNull = isNullable(propertyInfo.PropertyType)
				};

				dt.Columns.Add(col);
			}
		}

		private static bool isNullable(Type type)
		{
			return Nullable.GetUnderlyingType(type) != null;
		}
	}
}

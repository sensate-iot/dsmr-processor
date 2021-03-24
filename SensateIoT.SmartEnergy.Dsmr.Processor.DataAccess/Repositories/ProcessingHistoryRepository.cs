﻿using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories
{
	public class ProcessingHistoryRepository : AbstractRepository, IProcessingHistoryRepository
	{
		private const string DsmrProcessor_SelectLastProcessedBySensorId = "DsmrProcessor_SelectLastProcessedBySensorId";
		private const string DsmrProcessor_InsertProcessingTimestamp = "DsmrProcessor_InsertProcessingTimestamp";

		private readonly ISystemClock m_clock;

		public ProcessingHistoryRepository(string connection, ISystemClock clock) : base(new SqlConnection(connection))
		{
			this.m_clock = clock;
		}

		public async Task<ProcessingTimestamp> GetLastProcessingTimestamp(string sensorId)
		{
			var result = await this.QuerySingleAsync<ProcessingTimestamp>(DsmrProcessor_SelectLastProcessedBySensorId,
				"@sensorId", sensorId).ConfigureAwait(false);

			if(result == null) {
				var now = this.m_clock.GetCurrentTime();

				result = new ProcessingTimestamp {
					End = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc),
					SensorId = sensorId,
					Start = DateTime.MinValue
				};
			}

			return result;
		}

		public async Task CreateProcessingTimestamp(int sensorId, int count, DateTime start, DateTime end, CancellationToken ct)
		{
			await this.ExecuteAsync(DsmrProcessor_InsertProcessingTimestamp,
			                        "@sensorId", sensorId,
			                        "@count", count,
			                        "@start", start,
			                        "@end", end).ConfigureAwait(false);
		}
	}
}

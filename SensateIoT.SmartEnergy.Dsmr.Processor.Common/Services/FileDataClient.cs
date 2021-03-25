using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	/// <summary>
	/// File data client, used mainly for testing purposes.
	/// </summary>
	public sealed class FileDataClient : IDataClient
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(FileDataClient));

		private readonly string m_directory;

		public FileDataClient(string directory)
		{
			this.m_directory = directory;
		}

		public Task<IEnumerable<Measurement>> GetRangeAsync(string sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var path = $"{this.m_directory}{Path.DirectorySeparatorChar}{sensorId}.json";
			var data = File.ReadAllText(path);
			var measurements = JsonConvert.DeserializeObject<IEnumerable<Measurement>>(data);

			logger.Info($"Loaded measurements for sensor {sensorId}.");

			return Task.FromResult(filterResults(measurements, start, end));
		}

		private static IEnumerable<Measurement> filterResults(IEnumerable<Measurement> input, DateTime start, DateTime end)
		{
			return input.Where(measurement => measurement.Timestamp >= start && measurement.Timestamp <= end).ToList();
		}

		public void Dispose()
		{
		}
	}
}

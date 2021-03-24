using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Settings;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public sealed class DataApiClient : IDataClient
	{
		private readonly HttpClient m_client;
		private readonly AppConfig m_config;

		private const string LookupPath = "/data/v1/measurements";

		public DataApiClient(AppConfig config)
		{
			this.m_client = new HttpClient();
			this.m_config = config;

			this.m_client.DefaultRequestHeaders.Accept.Clear();
			this.m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			this.m_client.DefaultRequestHeaders.Add("X-ApiKey", config.SensateIoTApiKey);
		}

		public async Task<IEnumerable<Measurement>> GetRangeAsync(string sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var builder = new UriBuilder($"{this.m_config.SensateIoTDataApiBase}{LookupPath}");
			var query = HttpUtility.ParseQueryString(builder.Query);

			query["sensorid"] = sensorId;
			query["start"] = start.ToString("O");
			query["end"] = end.ToString("O");
			query["order"] = "asc";
			builder.Query = query.ToString();

			var result = await this.m_client.GetAsync(builder.Uri, ct).ConfigureAwait(false);
			var json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			// TODO: error handling
			return JsonConvert.DeserializeObject<IEnumerable<Measurement>>(json);
		}

		public void Dispose()
		{
			this.m_client.Dispose();
		}
	}
}

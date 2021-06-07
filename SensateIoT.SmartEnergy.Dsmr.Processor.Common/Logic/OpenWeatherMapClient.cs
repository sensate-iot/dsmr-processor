using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using log4net;
using Newtonsoft.Json;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Logic
{
	public sealed class OpenWeatherMapClient : IOpenWeatherMapClient
	{
		private readonly HttpClient m_client;
		private static ILog logger = LogManager.GetLogger(nameof(OpenWeatherMapClient));

		private const string BaseUri = "https://api.openweathermap.org";

		public OpenWeatherMapClient()
		{
			this.m_client = new HttpClient();

			this.m_client.DefaultRequestHeaders.Accept.Clear();
			this.m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<Weather> GetCurrentWeatherAsync(QueryParameters @params, CancellationToken ct)
		{
			var json = await this.performLookup(@params, ct).ConfigureAwait(false);
			var obj = JsonConvert.DeserializeObject<Weather>(json);

			if(obj?.Id == 0) {
				logger.Warn("City ID 0. Coordinates possibly wrong!");
				logger.Info($"Response: {json}");
			}

			return obj;
		}

		private async Task<string> performLookup(QueryParameters @params, CancellationToken ct)
		{
			var builder = new UriBuilder($"{BaseUri}/data/2.5/weather");
			var query = HttpUtility.ParseQueryString(builder.Query);

			query["appid"] = @params.Key;
			query["lat"] = @params.Latitude.ToString("F");
			query["lon"] = @params.Longitude.ToString("F");
			query["units"] = @params.UnitSystem;

			builder.Query = query.ToString();
			var result = await this.m_client.GetAsync(builder.Uri, ct).ConfigureAwait(false);

			if(!result.IsSuccessStatusCode) {
				return null;
			}

			return await result.Content.ReadAsStringAsync().ConfigureAwait(false);
		}

		public void Dispose()
		{
			this.m_client.Dispose();
		}
	}
}
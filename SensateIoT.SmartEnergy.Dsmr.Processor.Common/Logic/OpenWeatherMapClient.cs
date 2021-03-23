using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Logic
{
	public class OpenWeatherMapClient : IOpenWeatherMapClient
	{
		private readonly HttpClient m_client;
		private const string BaseUri = "https://api.openweathermap.org";

		public OpenWeatherMapClient()
		{
			this.m_client = new HttpClient();

			this.m_client.DefaultRequestHeaders.Accept.Clear();
			this.m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<Weather> GetCurrentWeatherAsync(QueryParameters @params, CancellationToken ct)
		{
			var builder = new UriBuilder($"{BaseUri}/data/2.5/weather");
			var query = HttpUtility.ParseQueryString(builder.Query);

			query["appid"] = @params.Key;
			query["lat"] = @params.Latitude.ToString("F");
			query["lon"] = @params.Longitude.ToString("F");
			query["units"] = @params.UnitSystem;

			builder.Query = query.ToString();
			var result = await this.m_client.GetAsync(builder.Uri, ct).ConfigureAwait(false);
			var json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			return JsonConvert.DeserializeObject<Weather>(json);
		}

		public void Dispose()
		{
			this.m_client?.Dispose();
		}
	}
}
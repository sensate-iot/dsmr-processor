using System.Threading;
using System.Threading.Tasks;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Settings;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public class WeatherService : IWeatherService
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(WeatherService));

		private readonly IWeatherCache m_cache;
		private readonly IOpenWeatherMapClient m_client;
		private readonly AppConfig m_config;

		public WeatherService(IWeatherCache cache, IOpenWeatherMapClient client, AppConfig config)
		{
			this.m_config = config;
			this.m_client = client;
			this.m_cache = cache;
		}

		public async Task<WeatherData> LookupAsync(WeatherLookup lookup, CancellationToken ct)
		{
			var cachedValue = this.m_cache[lookup.SensorId];

			if(cachedValue == null) {
				logger.Info($"Weather data not cached for sensor {lookup.SensorId}.");
				var result = await this.m_client.GetCurrentWeatherAsync(new QueryParameters {
					Key = this.m_config.OpenWeatherMapApiKey,
					UnitSystem = "metric",
					Latitude = lookup.Latitude,
					Longitude = lookup.Longitude
				}).ConfigureAwait(false);

				if(result.Id == 0) {
					logger.Warn("Unable to lookup weather.");
					return null;
				}

				logger.Info($"Got response from OpenWeatherMap API with ID {result.Id}.");

				cachedValue = result.Data;
				this.m_cache[lookup.SensorId] = result.Data;
			} else {
				logger.Info($"Using cached weather data for {lookup.SensorId}.");
			}

			return cachedValue;
		}

		public void Dispose()
		{
			this.m_cache.Dispose();
			this.m_client.Dispose();
		}
	}
}

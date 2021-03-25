using System;
using System.Collections.Generic;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Logic
{
	public sealed class WeatherCache : IWeatherCache
	{
		private readonly IDictionary<int, WeatherCacheValue> m_cache;
		private readonly ISystemClock m_clock;
		private readonly TimeSpan m_timeout;

		private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(60);

		public WeatherCache(ISystemClock clock) : this(clock, DefaultTimeout)
		{
			this.m_cache = new Dictionary<int, WeatherCacheValue>();
		}

		private WeatherCache(ISystemClock clock, TimeSpan timeout)
		{
			this.m_clock = clock;
			this.m_timeout = timeout;
		}

		public WeatherData this[int sensorId] {
			get => this.lookupBySensor(sensorId);
			set => this.setWeatherData(sensorId, value);
		}

		private void setWeatherData(int sensorId, WeatherData data)
		{
			var now = this.m_clock.GetCurrentTime();

			var value = new WeatherCacheValue {
				Value = data,
				Timeout = now.Add(this.m_timeout)
			};

			lock(this.m_cache) {
				this.m_cache[sensorId] = value;
			}
		}

		private WeatherData lookupBySensor(int sensorId)
		{
			lock(this.m_cache) {
				var success = this.m_cache.TryGetValue(sensorId, out var result);

				if(!success) {
					return null;
				}

				if(result.Timeout < this.m_clock.GetCurrentTime()) {
					return null;
				}

				return result.Value;
			}
		}

		public void Dispose()
		{
			this.m_cache.Clear();
		}
	}
}

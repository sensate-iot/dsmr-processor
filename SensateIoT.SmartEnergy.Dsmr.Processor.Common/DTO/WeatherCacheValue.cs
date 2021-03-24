using System;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract
{
	public class WeatherCacheValue
	{
		public DateTime Timeout { get; set; }
		public WeatherData Value { get; set; }
	}
}

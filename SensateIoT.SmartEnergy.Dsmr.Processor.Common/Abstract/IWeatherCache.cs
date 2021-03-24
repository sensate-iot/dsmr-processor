using System;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract
{
	public interface IWeatherCache : IDisposable
	{
		WeatherData this[int sensorId] { get; set; }
	}
}

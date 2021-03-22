﻿using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap
{
	public class Weather
	{
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "main")]
		public WeatherData Data { get; set; }
	}
}

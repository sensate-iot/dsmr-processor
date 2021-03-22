using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap
{
	public class WeatherData
	{
		[JsonProperty(PropertyName = "temp")]
		public double Temperature { get; set; }
		[JsonProperty(PropertyName = "feels_like")]
		public double FeelsLike { get; set; }
		[JsonProperty(PropertyName = "temp_min")]
		public double TempMin { get; set; }
		[JsonProperty(PropertyName = "temp_max")]
		public double TempMkax { get; set; }
		[JsonProperty(PropertyName = "pressure")]
		public int Pressure { get; set; }
		[JsonProperty(PropertyName = "humidity")]
		public int Humidity { get; set; }
	}
}
namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap
{
	public class QueryParameters
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Key { get; set; }
		public string UnitSystem { get; set; } = "metric";
	}
}

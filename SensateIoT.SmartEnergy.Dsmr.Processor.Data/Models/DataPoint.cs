using System;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models
{
	public class DataPoint
	{
		public int SensorId { get; set; }
		public decimal PowerUsage { get; set; }
		public decimal PowerProduction { get; set; }
		public decimal EnergyUsage { get; set; }
		public decimal EnergyProduction { get; set; }
		public decimal? GasUsage { get; set; }
		public decimal? GasFlow { get; set; }
		public decimal OutsideAirTemperature { get; set; }
		public decimal? Temperature { get; set; }
		public decimal? Pressure { get; set; }
		public decimal? RelativeHumidity { get; set; }
		public DateTime Timestamp { get; set; }
	}
}

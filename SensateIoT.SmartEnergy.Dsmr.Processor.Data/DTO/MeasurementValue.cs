namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO
{
	public class MeasurementValue
	{
		public decimal Value { get; set; }
		public decimal Precision { get; set; }
		public decimal Accuracy { get; set; }
		public string Unit { get; set; }
	}
}

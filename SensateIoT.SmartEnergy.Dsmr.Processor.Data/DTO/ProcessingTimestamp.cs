using System;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO
{
	public class ProcessingTimestamp
	{
		public int SensorId { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}

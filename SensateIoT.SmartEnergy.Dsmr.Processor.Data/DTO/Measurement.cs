using System;
using System.Collections.Generic;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO
{
	public class Measurement
	{
		public string SensorId { get; set; }
		public DateTime Timestamp { get; set; }
		public IDictionary<string, MeasurementValue> Data { get; set; }
	}
}
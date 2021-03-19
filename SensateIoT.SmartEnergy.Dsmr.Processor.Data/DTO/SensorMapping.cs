using System;
using JetBrains.Annotations;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO
{
	[UsedImplicitly]
	public class SensorMapping
	{
		public string PowerSensorId { get; set; }
		public string GasSensorId { get; set; }
		public string EnvironmentSensorId { get; set; }
		public DateTime LastProcessed { get; set; }
	}
}

using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO
{
	[UsedImplicitly]
	public class Location
	{
		public string Type { get; set; }
		public List<double> Coordinates { get; set; }

		[JsonIgnore]
		public double Longitude => this.Coordinates[0];
		[JsonIgnore]
		public double Latitude => this.Coordinates[1];
	}
}

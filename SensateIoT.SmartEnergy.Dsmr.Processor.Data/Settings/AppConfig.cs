﻿namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.Settings
{
	public class AppConfig
	{
		public AppMode Mode { get; set; }
		public DebugSettings DebugSettings { get; set; }

		public string DsmrProcessingDb { get; set; }
	}
}
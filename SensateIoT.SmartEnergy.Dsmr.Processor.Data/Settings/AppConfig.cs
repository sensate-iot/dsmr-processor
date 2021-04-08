namespace SensateIoT.SmartEnergy.Dsmr.Processor.Data.Settings
{
	public class AppConfig
	{
		public AppMode Mode { get; set; }
		public DebugSettings DebugSettings { get; set; }
		public string DsmrOlapConnectionString { get; set; }
		public string DsmrProductConnectionString { get; set; }
		public string OpenWeatherMapApiKey { get; set; }
		public string SensateIoTApiKey { get; set; }
		public string SensateIoTDataApiBase { get; set; }
		public string ServiceName { get; set; }
	}
}

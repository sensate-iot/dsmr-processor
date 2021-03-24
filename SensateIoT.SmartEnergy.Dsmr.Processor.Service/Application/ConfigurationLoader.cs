using System;
using System.Configuration;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Settings;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Service.Application
{
	public static class ConfigurationLoader
	{
		public static AppConfig LoadConfiguration()
		{
			var mode = ConfigurationManager.AppSettings["mode"];
			var conf = new AppConfig {
				Mode = AppMode.Normal,
				OpenWeatherMapApiKey = ConfigurationManager.AppSettings["openWeatherMapApiKey"],
				SensateIoTApiKey = ConfigurationManager.AppSettings["sensateIoTApiKey"],
				SensateIoTDataApiBase = ConfigurationManager.AppSettings["dataApiBase"]
			};

			if(mode == "debug") {
				conf.Mode = AppMode.Debug;
				loadDebugConfiguration(conf);
			}

			loadConnectionStrings(conf);
			return conf;
		}

		private static void loadConnectionStrings(AppConfig conf)
		{
			conf.DsmrProcessingDb = ConfigurationManager.ConnectionStrings["DsmrProcessing"].ConnectionString;
		}

		private static void loadDebugConfiguration(AppConfig conf)
		{
			conf.DebugSettings = new DebugSettings {
				Clock = DateTime.Parse(ConfigurationManager.AppSettings["debugTime"]),
				DataDirectory = ConfigurationManager.AppSettings["dataDirectory"]
			};
		}
	}
}

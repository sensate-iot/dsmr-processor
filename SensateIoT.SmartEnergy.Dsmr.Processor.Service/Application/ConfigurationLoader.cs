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
				OpenWeatherMapApiKey = loadRequiredSetting("openWeatherMapApiKey"),
				SensateIoTApiKey = loadRequiredSetting("sensateIoTApiKey"),
				SensateIoTDataApiBase = loadRequiredSetting("dataApiBase")
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

			if(string.IsNullOrEmpty(conf.DsmrProcessingDb)) {
				throw new InvalidOperationException("Missing the DSMR database connection string.");
			}
		}


		private static void loadDebugConfiguration(AppConfig conf)
		{
			var timestamp = loadRequiredSetting("debugTime");

			conf.DebugSettings = new DebugSettings {
				Clock = DateTime.Parse(timestamp),
				DataDirectory = loadRequiredSetting("dataDirectory")
			};
		}

		private static string loadRequiredSetting(string name)
		{
			var result = ConfigurationManager.AppSettings[name];

			if(string.IsNullOrEmpty(result)) {
				throw new InvalidOperationException($"The configuration parameter {name} is required but not present.");
			}

			return result;
		}
	}
}

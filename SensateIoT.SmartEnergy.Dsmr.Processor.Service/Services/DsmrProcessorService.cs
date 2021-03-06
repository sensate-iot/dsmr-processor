using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Logic;
using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.Settings;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Clocks;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories;
using SensateIoT.SmartEnergy.Dsmr.Processor.Service.Application;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Service.Services
{
	public sealed class DsmrProcessorService : IDisposable
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(DsmrProcessorService));

		private Thread m_serviceThread;
		private readonly IList<TimedBackgroundService> m_timers;
		private IProcessingService m_processor;
		private readonly TimeSpan m_timeout;
		private readonly AppConfig m_config;

		public DsmrProcessorService()
		{
			this.m_timers = new List<TimedBackgroundService>();
			this.m_timeout = TimeSpan.FromSeconds(10);
			this.m_config = ConfigurationLoader.LoadConfiguration();
		}

		public void Start(CancellationToken ct)
		{
			logger.Info("Starting DSMR processor service hosting...");

			this.InternalStart();

			var bg = new Thread(() => {
				try {
					logger.Info("Processor service started.");
					this.InternalRunAsync(ct).GetAwaiter().GetResult();
				} catch(OperationCanceledException ex) {
					logger.Info("Operation cancelled: stop requested.", ex);
				} catch(Exception ex) {
					logger.Fatal("Unable to continue application flow.", ex);
					throw;
				}
			}) { IsBackground = true };

			bg.Start();
			this.m_serviceThread = bg;
		}

		public void Stop()
		{
			logger.Warn("Stopping DSMR processor service...");
			this.InternalStop();
			this.m_serviceThread.Join();
			logger.Warn("DSMR processor service stopped.");
		}

		private IDataClient createDataClient()
		{
			if(this.m_config.Mode == AppMode.Debug) {
				return new FileDataClient(this.m_config.DebugSettings.DataDirectory);
			}

			return new DataApiClient(this.m_config);
		}

		private ISystemClock createClock()
		{
			if(this.m_config.Mode == AppMode.Debug) {
				return new DebugSystemClock(this.m_config.DebugSettings.Clock);
			}

			return new SystemClock();
		}

		private void InternalStart()
		{
			var clock = this.createClock();
			var cache = new WeatherCache(clock);

			this.m_processor = new ProcessingService(this.m_config.ServiceName,
			                                         new SensorMappingRepository(this.m_config.DsmrProductConnectionString),
			                                         new DataPointRepository(this.m_config.DsmrOlapConnectionString),
			                                         new ProcessingHistoryRepository( this.m_config.DsmrOlapConnectionString, clock),
			                                         this.createDataClient(),
													 clock,
			                                         new WeatherService(cache, new OpenWeatherMapClient(), this.m_config));
			this.m_timers.Add(new DataReloadService(this.m_processor, 
			                                        TimeSpan.Zero, 
			                                        TimeSpan.FromSeconds(60)));
		}

		private void InternalStop()
		{
			logger.Debug("Stopping timer tasks.");
			foreach(var timedBackgroundService in this.m_timers) {
				timedBackgroundService.Stop();
			}
		}

		private async Task InternalRunAsync(CancellationToken ct)
		{
			do {
				this.m_processor.Process(ct);
				await Task.Delay(this.m_timeout, ct).ConfigureAwait(false);
			} while(!ct.IsCancellationRequested);
		}

		public void Dispose()
		{
			foreach(var timedBackgroundService in this.m_timers) {
				timedBackgroundService.Dispose();
			}

			this.m_processor.Dispose();
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Clocks;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Repositories;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Service.Services
{
	public class DsmrProcessorService : IDisposable
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(DsmrProcessorService));

		private Thread m_serviceThread;
		private readonly IList<TimedBackgroundService> m_timers;
		private IProcessingService m_processor;
		private readonly TimeSpan m_timeout;
		private ISystemClock m_clock;

		private const string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;;Initial Catalog=DsmrProcessing;Integrated Security=True;";

		public DsmrProcessorService()
		{
			this.m_timers = new List<TimedBackgroundService>();
			this.m_timeout = TimeSpan.FromSeconds(10);
			this.m_clock = new DebugSystemClock(new DateTime(2021, 3, 13, 12, 0, 0, 0, DateTimeKind.Utc));
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

		private void InternalStart()
		{
			this.m_processor = new ProcessingService(new SensorMappingRepository(ConnectionString), 
			                                         new ProcessingHistoryRepository(ConnectionString, this.m_clock),
			                                         new FileDataClient("C:\\Users\\miche\\Documents\\Sensate\\Development\\data"),
			                                         this.m_clock);
			this.m_timers.Add(new DataReloadService(this.m_processor, 
			                                        TimeSpan.Zero, 
			                                        TimeSpan.FromSeconds(60)));
		}

		private void InternalStop()
		{
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
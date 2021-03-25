using System;
using System.ServiceProcess;
using System.Threading;

using log4net;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Service.Services
{
	public sealed class WindowsService : ServiceBase
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(WindowsService));

		private readonly CancellationTokenSource m_source;
		private readonly DsmrProcessorService m_processor;

		public WindowsService()
		{
			this.m_processor = new DsmrProcessorService();
			this.m_source = new CancellationTokenSource();
		}

		protected override void OnStart(string[] args)
		{
			this.StartService();
		}

		protected override void OnStop()
		{
			this.StopService();
		}

		public void StartService()
		{
			try {
				this.m_processor.Start(this.m_source.Token);
			} catch(Exception ex) {
				logger.Fatal("Unable to run the DSMR processor service. Fatalling.", ex);
				throw;
			}
		}

		public void StopService()
		{
			logger.Info("Stop signal received.");
			this.m_source.Cancel();
			this.m_processor.Stop();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if(!disposing) {
				return;
			}

			this.m_source.Dispose();
			this.m_processor.Dispose();
		}
	}
}

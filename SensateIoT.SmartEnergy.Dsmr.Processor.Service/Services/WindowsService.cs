using System;
using System.Threading;
using log4net;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Service.Services
{
	public sealed class WindowsService : IDisposable
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(WindowsService));

		private readonly CancellationTokenSource m_source;
		private readonly DsmrProcessorService m_processor;

		public WindowsService()
		{
			this.m_processor = new DsmrProcessorService();
			this.m_source = new CancellationTokenSource();
		}

		public void StartService()
		{
			try {
				this.m_processor.Start(this.m_source.Token);
			} catch(Exception ex) {
				logger.Fatal("Unable to parse configuration file. Fatalling.", ex);
				throw;
			}
		}

		public void StopService()
		{
			this.m_source.Cancel();
			this.m_processor.Stop();
		}

		public void Dispose()
		{
			this.m_source.Dispose();
			this.m_processor.Dispose();
		}
	}
}
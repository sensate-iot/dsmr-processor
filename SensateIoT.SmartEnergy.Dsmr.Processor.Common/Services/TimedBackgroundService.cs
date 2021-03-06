using System;
using System.Threading;
using System.Threading.Tasks;

using log4net;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public abstract class TimedBackgroundService : IDisposable
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(TimedBackgroundService));

		private readonly Timer m_timer;
		private readonly CancellationTokenSource m_source;

		protected TimedBackgroundService(TimeSpan startDelay, TimeSpan interval)
		{
			this.m_timer = new Timer(this.Invoke, null, startDelay, interval);
			this.m_source = new CancellationTokenSource();
		}

		private void Invoke(object argument)
		{
			try {
				this.InvokeAsync(this.m_source.Token).GetAwaiter().GetResult();
			} catch(Exception ex) {
				logger.Fatal("Unable to executed timed background service.", ex);
				Environment.Exit(1);
			}
		}

		protected abstract Task InvokeAsync(CancellationToken ct);

		public void Stop()
		{
			this.m_source.Cancel();
			this.m_timer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing) {
				this.m_source.Cancel();
				this.m_timer.Dispose();
				this.m_source.Dispose();
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}

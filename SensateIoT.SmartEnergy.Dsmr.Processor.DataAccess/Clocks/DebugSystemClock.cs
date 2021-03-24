using System;
using System.Threading;

using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Clocks
{
	public class DebugSystemClock : ISystemClock
	{
		private readonly Timer m_timer;
		private DateTime m_currentTime;
		private readonly object m_lock;

		public DebugSystemClock(DateTime start)
		{
			this.m_timer = new Timer(this.Invoke, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
			this.m_lock = new object();
			this.m_currentTime = start;
		}

		private void Invoke(object arg)
		{
			lock(this.m_lock) {
				this.m_currentTime = this.m_currentTime.AddSeconds(1);
			}
		}

		public DateTime GetCurrentTime()
		{
			lock(this.m_lock) {
				return this.m_currentTime;
			}
		}

		public void Dispose()
		{
			this.m_timer.Dispose();
		}
	}
}
using System;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Clocks
{
	public class SystemClock : ISystemClock
	{
		public DateTime GetCurrentTime()
		{
			return DateTime.UtcNow;
		}

		public void Dispose()
		{
		}
	}
}

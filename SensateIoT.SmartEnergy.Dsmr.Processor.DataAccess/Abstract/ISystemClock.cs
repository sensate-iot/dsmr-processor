using System;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract
{
	public interface ISystemClock
	{
		DateTime GetCurrentTime();
	}
}
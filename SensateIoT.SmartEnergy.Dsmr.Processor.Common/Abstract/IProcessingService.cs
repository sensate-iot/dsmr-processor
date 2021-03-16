using System;
using System.Threading;
using System.Threading.Tasks;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract
{
	public interface IProcessingService : IDisposable
	{
		Task LoadSensorMappingsAsync(CancellationToken ct);
		Task ProcessAsync(CancellationToken ct);
	}
}

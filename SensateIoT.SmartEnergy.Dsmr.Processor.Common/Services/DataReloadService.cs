using System;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public class DataReloadService : TimedBackgroundService
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(DataReloadService));
		private readonly IProcessingService m_processor;

		public DataReloadService(IProcessingService processor,
								 TimeSpan startDelay, TimeSpan interval) : base(startDelay, interval)
		{
			this.m_processor = processor;
		}

		protected override async Task InvokeAsync(CancellationToken ct)
		{
			logger.Info("Data reload (sensor mappings) started.");
			await this.m_processor.LoadSensorMappingsAsync(ct).ConfigureAwait(false);
			logger.Info("Finished loading sensor mappings.");
		}
	}
}

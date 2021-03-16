using System;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Services
{
	public class DataReloadService : TimedBackgroundService
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(DataReloadService));
		private readonly IProcessingService m_processor;
		private readonly ISensorMappingRepository m_repo;

		public DataReloadService(IProcessingService processor,
								 ISensorMappingRepository repo,
								 TimeSpan startDelay, TimeSpan interval) : base(startDelay, interval)
		{
			this.m_processor = processor;
			this.m_repo = repo;
		}

		protected override async Task InvokeAsync(CancellationToken ct)
		{
			logger.Info("Data reload started!");
			var sensors = await this.m_repo.GetAllSensorsAsync(ct).ConfigureAwait(false);
			//this.m_processor.LoadSensorMappingsAsync()
		}
	}
}

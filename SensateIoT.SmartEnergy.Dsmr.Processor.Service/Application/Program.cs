using System;
using System.ServiceProcess;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Mappers;
using SensateIoT.SmartEnergy.Dsmr.Processor.Service.Services;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Service.Application
{
    public class Program
    {
	    private static readonly ILog logger = LogManager.GetLogger(nameof(Program));

        public static void Main(string[] args)
        {
			logger.Warn("Starting DSMR processing service.");

			DateTimeTypeHandler.SetupDateTimeHandlers();

			if(Environment.UserInteractive) {
				StartInteractive();
			} else {
				using(var svc = new WindowsService()) {
					ServiceBase.Run(svc);
				}
			}
        }

        private static void StartInteractive()
        {
	        var svc = new WindowsService();
	        var host = new ConsoleHost(svc);

	        host.Run();
	        Console.WriteLine("Press <ENTER> to exit.");
	        Console.ReadLine();
        }
	}
}

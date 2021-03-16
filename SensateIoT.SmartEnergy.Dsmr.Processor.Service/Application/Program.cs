using System;
using System.ServiceProcess;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Processor.Service.Services;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Service.Application
{
    public class Program
    {
	    private const string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;;Initial Catalog=DsmrProcessing;Integrated Security=True;";
	    private static readonly ILog logger = LogManager.GetLogger(nameof(Program));

        public static void Main(string[] args)
        {
			logger.Warn("Starting DSMR processing service.");

			/*var repo = new DataPointRepository(ConnectionString);
			var dp = new DataPoint {
				SensorId = 1,
				PowerProduction = 1354.3615M,
				OutsideAirTemperature = 12.543M,
				EnergyProduction = 3432.36M,
				EnergyUsage = 1234.634M,
				GasFlow = 43.232M,
				GasUsage = 1.000M,
				PowerUsage = 98.2345M,
				Timestamp = new DateTime(2021, 3, 15, 20, 1, 0, 0)
			};

	        repo.CreateDataPointAsync(dp, CancellationToken.None).GetAwaiter().GetResult();*/
			/*var fileClient = new FileDataClient("C:\\Users\\miche\\Documents\\Sensate\\Development\\data");
			var start = new DateTime(2021, 3, 13, 13, 0, 0, DateTimeKind.Utc);
			var end = start.AddMinutes(5);
			var result = fileClient.GetRange("60496aa759001279d9859952", start, end, CancellationToken.None)
				.GetAwaiter().GetResult();*/

			if(Environment.UserInteractive) {
				StartInteractive();
			} else {
				//using(var svc = new WindowsService()) {
					//ServiceBase.Run(svc);
				//}
			}
			Console.ReadLine();
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

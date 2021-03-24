using System;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract
{
	public interface IWeatherService : IDisposable
	{
		Task<WeatherData> LookupAsync(WeatherLookup lookup, CancellationToken ct);
	}
}

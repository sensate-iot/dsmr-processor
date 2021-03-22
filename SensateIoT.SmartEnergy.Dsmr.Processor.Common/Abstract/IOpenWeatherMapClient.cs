using System;
using System.Threading.Tasks;
using SensateIoT.SmartEnergy.Dsmr.Processor.Data.DTO.OpenWeatherMap;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.Common.Abstract
{
	public interface IOpenWeatherMapClient : IDisposable
	{
		Task<Weather> GetCurrentWeatherAsync(QueryParameters @params);
	}
}
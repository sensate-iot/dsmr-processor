using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using DataPoint = SensateIoT.SmartEnergy.Dsmr.Processor.Data.Models.DataPoint;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Abstract
{
	public interface IDataPointRepository
	{
		Task CreateDataPointAsync(DataPoint dp, CancellationToken ct = default);
		Task CreateBulkDataPointsAsync(IEnumerable<DataPoint> dp, CancellationToken ct = default);
	}
}

using System;
using System.Data;

using Dapper;

namespace SensateIoT.SmartEnergy.Dsmr.Processor.DataAccess.Mappers
{
	public class DateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
	{
		public override void SetValue(IDbDataParameter parameter, DateTime value)
		{
			parameter.Value = value;
		}

		public override DateTime Parse(object value)
		{
			return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
		}

		public static void SetupDateTimeHandlers()
		{
			SqlMapper.AddTypeHandler(new DateTimeTypeHandler());
		}
	}
}
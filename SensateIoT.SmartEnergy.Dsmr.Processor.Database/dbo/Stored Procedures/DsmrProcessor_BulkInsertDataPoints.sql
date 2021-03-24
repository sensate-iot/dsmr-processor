CREATE PROCEDURE [dbo].[DsmrProcessor_BulkInsertDataPoints]
	@data [DataPoint] READONLY
AS
BEGIN
	INSERT INTO [dbo].[DataPoints] (
		[SensorId],
		[PowerUsage],
		[PowerProduction],
		[EnergyUsage],
		[EnergyProduction],
		[GasUsage],
		[GasFlow],
		[OutsideAirTemperature],
		[Temperature],
		[Pressure],
		[RH],
		[Timestamp]
	)
	SELECT [SensorId],
		   [PowerUsage],
		   [PowerProduction],
		   [EnergyUsage],
		   [EnergyProduction],
		   [GasUsage],
		   [GasFlow],
		   [OutsideAirTemperature],
		   [Temperature],
		   [Pressure],
		   [RH],
		   [Timestamp]
	FROM @data
END

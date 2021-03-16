﻿CREATE PROCEDURE [dbo].[DsmrProcessor_InsertDataPoint]
	@sensorId INT,
	@powerUsage NUMERIC(38, 12),
	@powerProduction NUMERIC(38, 12),
	@energyUsage NUMERIC(38, 12),
	@energyProduction NUMERIC(38, 12),
	@gasUsage NUMERIC(38, 12),
	@gasFlow NUMERIC(38, 12),
	@oat NUMERIC(38, 12),
	@date DATETIME
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
		[Timestamp]
	) VALUES (
		@sensorId,
		@powerUsage,
		@powerProduction,
		@energyUsage,
		@energyProduction,
		@gasUsage,
		@gasFlow,
		@oat,
		@date
	);
END
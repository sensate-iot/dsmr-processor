CREATE TYPE [DataPoint] AS TABLE (
	[SensorId]					INT				NOT NULL,
	[PowerUsage]				NUMERIC(38, 6)  NOT NULL,
	[PowerProduction]			NUMERIC(38, 6)  NOT NULL,
	[EnergyUsage]				NUMERIC(38, 6)  NOT NULL,
	[EnergyProduction]			NUMERIC(38, 6)  NOT NULL,
	[GasUsage]					NUMERIC(38, 6)  NULL,
	[GasFlow]					NUMERIC(38, 6)  NULL,
	[OutsideAirTemperature]		NUMERIC(38, 6)  NULL,
	[Temperature]				NUMERIC(38, 6)  NULL,
	[Pressure]					NUMERIC(38, 6)  NULL,
	[RH]						NUMERIC(38, 6)  NULL,
	[Timestamp]					DATETIME		NOT NULL DEFAULT(GETDATE())
)
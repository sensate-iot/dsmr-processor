CREATE TABLE [dbo].[DataPoints]
(
	[Id]						BIGINT			NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	[SensorId]					INT				NOT NULL,
	[PowerUsage]				NUMERIC(38, 6)  NOT NULL,
	[PowerProduction]			NUMERIC(38, 6)  NOT NULL,
	[EnergyUsage]				NUMERIC(38, 6)  NOT NULL,
	[EnergyProduction]			NUMERIC(38, 6)  NOT NULL,
	[GasUsage]					NUMERIC(38, 6)  NULL,
	[GasFlow]					NUMERIC(38, 6)  NULL,
	[OutsideAirTemperature]		NUMERIC(38, 6)  NOT NULL,
	[Temperature]				NUMERIC(38, 6),
	[Pressure]					NUMERIC(38, 6),
	[RH]						NUMERIC(38, 6),
	[Timestamp]					DATETIME		NOT NULL DEFAULT(GETDATE())

	CONSTRAINT [PK_DataPoints] PRIMARY KEY NONCLUSTERED ([Id] ASC),
	CONSTRAINT [FK_DataPoints_SensorMapping] FOREIGN KEY ([SensorId]) REFERENCES [SensorMapping] ([Id])
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	INDEX [IX_DataPointsSensorId] NONCLUSTERED ([SensorId]),
	INDEX [IX_DataPointsTimestamp] CLUSTERED ([Timestamp])
)

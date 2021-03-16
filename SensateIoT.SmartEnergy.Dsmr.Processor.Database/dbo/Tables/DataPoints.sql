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
	[Timestamp]					DATETIME		NOT NULL DEFAULT(GETDATE())

	CONSTRAINT [PK_DataPoints] PRIMARY KEY NONCLUSTERED ([Id] ASC),
	CONSTRAINT [FK_DataPoints_SensorMapping] FOREIGN KEY ([SensorId]) REFERENCES [SensorMapping] ([Id])
		ON UPDATE CASCADE,
	INDEX [IX_DataPointsSensorId] NONCLUSTERED ([SensorId])
)


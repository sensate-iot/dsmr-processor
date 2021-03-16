CREATE TABLE [dbo].[SensorMapping]
(
	[Id]						INT				NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	[PowerSensorId]				VARCHAR(24)		NOT NULL,
	[GasSensorId]				VARCHAR(24),
	[EnvironmentSensorId]		VARCHAR(24),
	[ServiceName]				VARCHAR(32)		NOT NULL,
	[Enabled]					BIT				NOT NULL DEFAULT(1)

	CONSTRAINT [PK_SensorMapping] PRIMARY KEY NONCLUSTERED ([Id] ASC),
	INDEX [IX_SensorMapping_PowerSensorId] UNIQUE NONCLUSTERED ([PowerSensorId]),
	INDEX [IX_SensorMapping_ServiceName] UNIQUE NONCLUSTERED ([ServiceName])
)

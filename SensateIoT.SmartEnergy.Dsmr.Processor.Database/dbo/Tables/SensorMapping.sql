CREATE TABLE [dbo].[SensorMapping]
(
	[Id]						INT				NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	[SensorId]					VARCHAR(24)		NOT NULL,

	CONSTRAINT [PK_SensorMapping] PRIMARY KEY NONCLUSTERED ([Id] ASC),
	INDEX [IX_SensorMapping_SensorId] UNIQUE NONCLUSTERED ([SensorId])
)

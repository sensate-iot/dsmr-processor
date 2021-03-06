CREATE TABLE [dbo].[ProcessingHistory]
(
	[Id]						BIGINT			NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	[SensorId]					INT				NOT NULL,
	[Count]						INT				NOT NULL DEFAULT(0),
	[Start]						DATETIME		NOT NULL,
	[End]						DATETIME		NOT NULL,
	[Timestamp]					DATETIME		NOT NULL DEFAULT(GETDATE())

	CONSTRAINT [PK_ProcessingHistory] PRIMARY KEY NONCLUSTERED ([Id] ASC),
	CONSTRAINT [FK_ProcessingHistory_SensorMapping] FOREIGN KEY ([SensorId]) REFERENCES [SensorMapping] ([Id])
		ON UPDATE CASCADE,
)

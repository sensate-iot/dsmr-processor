CREATE PROCEDURE [dbo].[DsmrProcessor_SelectLastProcessedBySensorId]
	@sensorId NVARCHAR(24)
AS
BEGIN
	SELECT TOP(1) @sensorId AS [SensorId],
			      [Start],
				  [End]
	FROM [dbo].[ProcessingHistory]
	INNER JOIN [dbo].[SensorMapping] m ON [m].[PowerSensorId] = @sensorId
	WHERE [SensorId] = [m].[Id]
	ORDER BY [End] DESC
END
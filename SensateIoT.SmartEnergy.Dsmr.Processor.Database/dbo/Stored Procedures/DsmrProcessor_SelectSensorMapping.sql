CREATE PROCEDURE [dbo].[DsmrProcessor_SelectSensorMapping]
	@sensorId NVARCHAR(24)
AS
BEGIN
	SELECT [SensorMapping].[Id], [SensorMapping].[SensorId]
	FROM [dbo].[SensorMapping]
	WHERE [SensorMapping].[SensorId] = @sensorId
END

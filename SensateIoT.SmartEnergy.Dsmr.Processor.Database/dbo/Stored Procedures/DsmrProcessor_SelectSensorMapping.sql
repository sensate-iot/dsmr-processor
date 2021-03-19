CREATE PROCEDURE [dbo].[DsmrProcessor_SelectSensorMapping]
	@sensorId NVARCHAR(24)
AS
BEGIN
	SELECT [PowerSensorId],
	       [GasSensorId],
	       [EnvironmentSensorId]
	FROM [dbo].[SensorMapping] m
	WHERE [PowerSensorId] = @sensorId
END

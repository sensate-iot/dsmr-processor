CREATE PROCEDURE [dbo].[DsmrProcessor_SelectSensorMapping]
	@sensorId NVARCHAR(24)
AS
BEGIN
	SELECT [Id],
	       [PowerSensorId],
	       [GasSensorId],
	       [EnvironmentSensorId]
	FROM [dbo].[SensorMapping] m
	WHERE [PowerSensorId] = @sensorId AND
	      [Enabled] = 1
END

CREATE PROCEDURE [dbo].[DsmrProcessor_SelectAllSensorMappings]
AS
BEGIN
	SELECT [PowerSensorId],
	       [GasSensorId],
	       [EnvironmentSensorId]
	FROM [dbo].[SensorMapping] m
END

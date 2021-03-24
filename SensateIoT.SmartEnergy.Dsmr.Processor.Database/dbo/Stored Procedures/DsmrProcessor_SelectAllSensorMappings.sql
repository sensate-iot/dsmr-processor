CREATE PROCEDURE [dbo].[DsmrProcessor_SelectAllSensorMappings]
AS
BEGIN
	SELECT [Id],
		   [PowerSensorId],
	       [GasSensorId],
	       [EnvironmentSensorId]
	FROM [dbo].[SensorMapping] m
	WHERE [m].[Enabled] = 1
END

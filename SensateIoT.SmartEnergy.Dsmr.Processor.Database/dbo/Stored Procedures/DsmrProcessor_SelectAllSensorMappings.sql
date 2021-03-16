CREATE PROCEDURE [dbo].[DsmrProcessor_SelectAllSensorMappings]
AS
BEGIN
	SELECT [SensorMapping].[Id],
	       [SensorMapping].[PowerSensorId],
	       [SensorMapping].[GasSensorId],
	       [SensorMapping].[EnvironmentSensorId]
	FROM [dbo].[SensorMapping]
END

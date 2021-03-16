﻿CREATE PROCEDURE [dbo].[DsmrProcessor_SelectSensorMapping]
	@sensorId NVARCHAR(24)
AS
BEGIN
	SELECT [SensorMapping].[Id],
	       [SensorMapping].[PowerSensorId],
	       [SensorMapping].[GasSensorId],
	       [SensorMapping].[EnvironmentSensorId]
	FROM [dbo].[SensorMapping]
	WHERE [SensorMapping].[PowerSensorId] = @sensorId
END
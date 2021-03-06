CREATE PROCEDURE [dbo].[DsmrProcessor_FinalizeBatch]
	@sensorId int,
	@count int,
	@start DATETIME,
	@end   DATETIME,
	@timestamp DATETIME
AS
BEGIN
	INSERT INTO [dbo].[ProcessingHistory] (
				  [SensorId]
				 ,[Count]
				 ,[Start]
				 ,[End]
				 ,[Timestamp]
	) VALUES (
		@sensorId,
		@count,
		@start,
		@end,
		@timestamp
	);
END

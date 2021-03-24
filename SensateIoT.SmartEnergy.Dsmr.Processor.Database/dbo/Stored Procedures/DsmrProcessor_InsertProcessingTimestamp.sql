CREATE PROCEDURE [dbo].[DsmrProcessor_InsertProcessingTimestamp]
	@sensorId INT,
	@count INT,
	@start DATETIME,
	@end DATETIME
AS
BEGIN
	INSERT INTO [dbo].[ProcessingHistory] (
		[SensorId],
		[Count],
		[Start],
		[End],
		[Timestamp]
	) VALUES (
		@sensorId,
		@count,
		@start,
		@end,
		GETDATE()
	);
END

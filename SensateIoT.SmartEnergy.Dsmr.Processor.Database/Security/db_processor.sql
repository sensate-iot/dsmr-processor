CREATE ROLE [db_processor];
GO

GRANT EXECUTE ON [dbo].[DsmrProcessor_FinalizeBatch] TO [db_processor]
GO
GRANT EXECUTE ON [dbo].[DsmrProcessor_SelectSensorMapping] TO [db_processor]
GO
GRANT EXECUTE ON [dbo].[DsmrProcessor_InsertDataPoint] TO [db_processor]
GO

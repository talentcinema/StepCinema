CREATE TABLE [dbo].[Periods]
(
	[PeriodId] INT IDENTITY(1,1) NOT NULL , 
	[PeriodName] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Periods] PRIMARY KEY ([PeriodId])
) 


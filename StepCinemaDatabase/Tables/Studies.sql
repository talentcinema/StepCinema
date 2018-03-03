CREATE TABLE [dbo].[Studies]
(
	[StudyId] INT IDENTITY(1,1) NOT NULL , 
	[StudyNumber] VARCHAR(100) NOT NULL,
    	[StudyName] VARCHAR(500) NOT NULL, 
	[NoOfPeriods] int NOT NULL,
	[SiteId] varchar(20) NOT NULL,
	[StudyStaus] int NOT NULL,
	[CreatedOn] DATETIME NOT NULL,
    	[UpdatedOn] DATETIME NULL, 
    	[UpdatedBy] INT NULL, 
    	[Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_Studies] PRIMARY KEY ([StudyId])
)

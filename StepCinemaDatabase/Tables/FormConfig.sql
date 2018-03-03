CREATE TABLE [dbo].[FormConfig]
(
	[FormId] VARCHAR(50) NOT NULL ,
	[FormName] NVARCHAR(500) NOT NULL,
	[PeriodId] INT NOT NULL,
	[FormGroupId] VARCHAR(50) NOT NULL,
	[StudyId] INT NOT NULL,
	[Order] INT NULL,
	[FormType] VARCHAR(20) NOT NULL,
	[Rows] INT NOT NULL, 
	[Display] bit NOT NULL  DEFAULT(1),
	[Disabled]	bit NOT NULL  DEFAULT(0), 
    CONSTRAINT [PK_FormConfig] PRIMARY KEY ([FormId], [PeriodId], [FormGroupId], [StudyId])
)

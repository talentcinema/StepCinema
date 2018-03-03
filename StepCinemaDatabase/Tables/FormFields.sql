CREATE TABLE [dbo].[FormFields]
(
	[FormId] VARCHAR(50) NOT NULL,
	[FieldId] VARCHAR(50) NOT NULL,
	[FieldLiteral] VARCHAR(200) NOT NULL,
	[FieldExportName] VARCHAR(50) NULL,
	[PeriodId] INT NOT NULL,
	[FormGroupId] VARCHAR(50) NOT NULL,
	[StudyId] INT NOT NULL,
	[FieldDataType] VARCHAR(20) NOT NULL,
	[FieldType] VARCHAR(20) NULL,
	[FieldMandatory] bit NOT NULL DEFAULT 0,
	[Order] INT NULL,
	[Display] BIT NOT NULL DEFAULT(1),
	[Disabled] BIT NOT NULL DEFAULT(0), 
    	[Unit] VARCHAR(20) NULL, 
    	[LOVId] VARCHAR(50) NULL, 
    	CONSTRAINT [PK_FormFields] PRIMARY KEY ([FormId], [FieldId], [PeriodId], [FormGroupId], [StudyId]) 
)

CREATE TABLE [dbo].[PrefilledFields]
(
	[FormId] VARCHAR(50) NOT NULL , 
    	[FieldId] VARCHAR(50) NOT NULL,
	[PeriodId] INT NOT NULL,
	[FormGroupId] VARCHAR(50) NOT NULL,
	[StudyId] INT NOT NULL,
	[RowIndex] INT NOT NULL,
	[FieldValue] NVARCHAR(1000) NULL, 
    PRIMARY KEY ([FormId], [FieldId], [PeriodId], [FormGroupId], [StudyId], [RowIndex]),
)

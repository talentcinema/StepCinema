CREATE TABLE [dbo].[FieldValues]
(
	[Id] BIGINT NOT NULL  IDENTITY(1, 1) PRIMARY KEY,
	[SubjectId] VARCHAR(50) NOT NULL,
	[StudyId] INT NOT NULL,
	[PeriodId] INT NOT NULL,
	[FormGroupId] VARCHAR(50) NOT NULL,
	[FormId] VARCHAR(50) NOT NULL,
	[FieldId] VARCHAR(50) NOT NULL,
	[RowIndex] INT NOT NULL,
	[EntryType] INT NOT NULL,
	[FieldValue] NVARCHAR(1000) NULL
)

GO

CREATE UNIQUE INDEX [IX_FieldValues_Combined] ON [dbo].[FieldValues] ([StudyId], [PeriodId], [FormGroupId], [FormId], [FieldId], [RowIndex], [EntryType], [SubjectId])

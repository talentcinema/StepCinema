CREATE TABLE [dbo].[CRFDetails]
(
	[DetailsId] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[StudyId] INT NOT NULL, 
    [SubjectId] VARCHAR(50) NOT NULL, 
	[ScreeningNo] VARCHAR(50) NOT NULL,
	[RegisteredNo] VARCHAR(50) NOT NULL,
    [Status] INT NOT NULL DEFAULT 0, 
    [ProtocoTitle] VARCHAR(1000) NULL
)

GO

CREATE UNIQUE INDEX [IX_CRFDetails_StudyIdSubjectId] ON [dbo].[CRFDetails] ([StudyId], [SubjectId])

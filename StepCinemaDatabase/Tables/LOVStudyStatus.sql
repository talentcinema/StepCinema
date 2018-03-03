CREATE TABLE [dbo].[LOVStudyStatus]
(
	[StudyStatusId] INT NOT NULL , 
	[StatusName] VARCHAR(50) NOT NULL, 
    [Active] BIT NULL, 
    CONSTRAINT [PK_LOVStudyStatus] PRIMARY KEY ([StudyStatusId]),
)

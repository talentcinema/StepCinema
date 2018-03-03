CREATE TABLE [dbo].[FormGroupPeriods] (
    [FormGroupId]   VARCHAR (50) NOT NULL,
    [PeriodId] INT          NOT NULL,
    [Order]    INT          NULL,
    [StudyId]  INT NOT NULL,
    [PageRef] VARCHAR(50) NULL, 
	[Active] BIT DEFAULT 1 NOT NULL,
    PRIMARY KEY CLUSTERED ([StudyId] ASC, [PeriodId] ASC, [FormGroupId] ASC)
);


GO

CREATE UNIQUE INDEX [IX_FormGroupPeriods_UniqueIndex] ON [dbo].[FormGroupPeriods] ([Order], [StudyId], [PeriodId])

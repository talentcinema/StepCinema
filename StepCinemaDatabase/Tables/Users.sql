CREATE TABLE [dbo].[Users] (
    [UserId]    INT IDENTITY (1, 1) NOT NULL,
    [Email]     NVARCHAR (255)  NOT NULL,
    [Password]  VARBINARY (MAX) NULL,
    [FirstName] NVARCHAR (50)   NULL,
    [LastName]  NVARCHAR (50)   NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);


GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [dbo].[Users] ([Email])

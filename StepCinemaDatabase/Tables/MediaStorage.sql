CREATE TABLE [dbo].[MediaStorage]
(
	[Id] INT IDENTITY(1,1),
	[ProfileId] INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[Size] BIGINT NOT NULL, 
	[Type] VARCHAR(20) NOT NULL,
	[Likes]  INT NOT NULL,
	[DisLikes]  INT  NULL,
	[ViewsNum] int NOT NULL,
	[ReviewComment] VARCHAR(5000) NULL,
	[CreatedDate] DATE NOT NULL, 
    CONSTRAINT [PK_MediaStorage] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_MediaStorage_Profile] FOREIGN KEY ([ProfileId]) REFERENCES [Profile]([Id]),
)

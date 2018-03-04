CREATE TABLE [dbo].[Category]
(
	[Id] INT IDENTITY(1,1),
	[Category_Name] NVARCHAR(200) NOT NULL, 
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[State]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryId] [INT] NOT NULL,
	[Name] [NVARCHAR](100) NOT NULL,
	[Published] [BIT] NOT NULL, 
    CONSTRAINT [PK_State] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryId]) REFERENCES [Country]([Id])
)

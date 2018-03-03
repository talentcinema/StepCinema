CREATE TABLE [dbo].[ListOfValues]
(
	[LOVId] VARCHAR(50) NOT NULL , 
	[Key] VARCHAR(50) NOT NULL, 
    [Value] VARCHAR(50) NULL, 
    [Order] INT NULL, 
    [Active] BIT NULL, 
    
    CONSTRAINT [PK_ListOfValues] PRIMARY KEY ([Key], [LOVId]) 
)

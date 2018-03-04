CREATE TABLE [dbo].[Address]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] int NOT NULL,	
	[FirstName] [nvarchar](4000) NOT NULL,
	[LastName] [nvarchar](4000) NOT NULL,
	[Email] [nvarchar](4000) NOT NULL,
	[CountryId] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[City] [nvarchar](4000) NOT NULL,
	[Address1] [nvarchar](4000) NULL,
	[Address2] [nvarchar](4000) NOT NULL,
	[PinCode] [nvarchar](10) NOT NULL,
	[PrimaryPhone] [nvarchar](10) NOT NULL,
	[SecondaryPhone] [nvarchar](10) NULL, 
    CONSTRAINT [PK_Address] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Address_User] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]), 
    CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryId]) REFERENCES [Country]([Id]), 
    CONSTRAINT [FK_Address_State] FOREIGN KEY ([StateId]) REFERENCES [State]([Id])
)

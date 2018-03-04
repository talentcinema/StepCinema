CREATE TABLE [dbo].[Profile]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Gender] BIT NOT NULL,
	[Dob] DATE NULL,
	[AddressId] INT NOT NULL,
	[Aboutme] VARCHAR(8000) NOT NULL,
	[Height] DECIMAL(2,2) NOT NULL,
	[Weight] DECIMAL(2,2) NOT NULL,
	[EyeColor] VARCHAR(20) NULL,
	[HairColor] VARCHAR(20)  NULL,
	[Build] VARCHAR(20) NOT NULL,
	[Age] INT NOT NULL,
	[HairLength] INT NULL,
	[SkinColor] VARCHAR(20) NOT NULL,
	[Languagespoken] VARCHAR(20) NOT NULL,
	[MainInterestId] INT NOT NULL, -- Fill from Category Table
	[IntrestedInId] INT NOT NULL, -- Fill from Interestedin Table
	[Testimonials] VARCHAR(4000) NOT NULL,
	[ProfileTitile] VARCHAR(100) NOT NULL,
	[Profile_Picture] VARCHAR(100) NOT NULL, 
    CONSTRAINT [PK_Profile] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Profile_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id]), 
    CONSTRAINT [FK_Profile_Category] FOREIGN KEY ([MainInterestId]) REFERENCES [Category]([Id]), 
    CONSTRAINT [FK_Profile_IntrestedIn] FOREIGN KEY ([IntrestedInId]) REFERENCES [InterestedIn]([Id])
)

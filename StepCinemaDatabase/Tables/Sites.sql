CREATE TABLE [dbo].[Sites]
(
	[SiteId] VARCHAR(20) NOT NULL PRIMARY KEY, 
    	[SiteName] VARCHAR(500) NOT NULL, 
    	[UpdatedOn] DATETIME NULL, 
    	[UpdatedBy] INT NULL
)

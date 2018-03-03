CREATE TABLE [dbo].[FileUpload]
(
	[FileUploadId] INT IDENTITY(1,1) NOT NULL ,
	[FileName] [nvarchar](255) NOT NULL,
	[IsVideo] BIT NULL DEFAULT 0 ,
	[UserId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedOn] DATETIME, 
    CONSTRAINT [PK_FileUpload] PRIMARY KEY ([FileUploadId])	
)

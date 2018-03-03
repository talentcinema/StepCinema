CREATE TABLE [dbo].[UserRoles]
(
	[UserId] INT NOT NULL , 
    [RoleId] VARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]) 
)

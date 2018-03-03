/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
SET IDENTITY_INSERT Users ON;
INSERT INTO Users(UserId, Email, FirstName, LastName, [Password], Active)
VALUES (1, 'admin@example.com', 'Admin', 'CRF', NULL, 1);
SET IDENTITY_INSERT Users ON;
GO


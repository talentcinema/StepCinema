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
SET IDENTITY_INSERT Studies ON;
INSERT INTO Studies(StudyId, StudyNumber, StudyName, SiteId, StudyStaus, NoOfPeriods, Active, CreatedOn)
VALUES (-100, -100, 'Reference Study', 'Site01', 1, 4, 1, GETDATE());
SET IDENTITY_INSERT Studies ON;

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
DELETE FROM [Periods];
SET IDENTITY_INSERT [Periods] ON;
INSERT INTO [Periods](PeriodId, PeriodName)
VALUES 
(0, 'Initial Forms'),
(1, 'Period 1'),
(2, 'Period 2'),
(3, 'Period 3'),
(4, 'Period 4'),
(100, 'Final Forms (Section 1)'),
(101, 'Final Forms (Section 2)');
SET IDENTITY_INSERT [Periods] OFF;


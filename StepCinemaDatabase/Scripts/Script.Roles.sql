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

INSERT INTO Roles (RoleId, RoleName)
VALUES 
('admin', 'Administrator'),
('dataentry', 'Data Entry Operator'),
('qc', 'QC Operator'),
('pi', 'Data Review Operator'),
('qa', 'Data Authorization Operator'),
('appadmin', 'Application Administrator');

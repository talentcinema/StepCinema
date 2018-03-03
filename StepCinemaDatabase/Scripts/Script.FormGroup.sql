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

INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('firstform','First Form',1,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('scfflft','Sample Collection Form for Liver function test',2,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('cimef','Check-in Medical Examination Form',3,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('pccf','Protocol compliance confirmation form',4,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('inclusionc','Inclusion Criteria',5,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('inclusioncf','Inclusion Criteria Female',6,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('exclusionc','Exclusion Criteria',7,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('eligibilityc','Eligibility Confirmation',8,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('checkinf','Check-in form',9,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('mealsf','Meals form',10,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('mealsfc','Meals form (Cont.)',11,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('sfcdaf','Suitability for concomitant drug administration form',12,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('cma','Concomitant medication administration',13,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('sfdaf','Suitability for drug administration form',14,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('daf','Drug administration form',15,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('rap','Restrictions and Prohibitions',16,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('120waf','120 mL of water administration form',17,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('soatwd01hpda','Supervision of access to washroom during 01 hour post drug administration',18,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('vf','Vitals form',19,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('vawbrf','Vitals and well-being record form',20,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('ecgf','ECG form',21,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('cmf','Cardiac monitoring form',22,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('wbf','Well-being form',23,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('ivcirf','IV cannula insertion / removal form',24,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('robs','Record of Blood Sampling ',25,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('robsc','Record of Blood Sampling (Cont.)',26,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('60gsaf','60 ml of 20% glucose solution administration form',27,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('bgmr','Blood glucose monitoring Record',28,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('comef','Check-out Medical Examination Form',29,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('cof','Check-out form',30,1)
INSERT INTO FormGroup (FormGroupId,FormGroupName,Order,Active) VALUES ('cofc','Check-out form (Cont.)',31,1)


	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

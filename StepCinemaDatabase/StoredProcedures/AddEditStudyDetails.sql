CREATE PROCEDURE [dbo].[AddStudyDetails]
	@studyNumber VARCHAR(100),
	@studyName VARCHAR(500),
	@noOfPeriods INT,
	@active bit,
	@result int out
AS
	DECLARE @NewSTudyID INT

	IF NOT Exists(SELECT 1 FROM Studies WHERE StudyNumber=@studyNumber or StudyName=@studyName)
	BEGIN 
		INSERT INTO Studies(StudyNumber,StudyName, NoOfPeriods,SiteId,StudyStaus,CreatedOn,Active) 
		VALUES(@studyNumber,@studyName,@noOfPeriods,'NCSblr1',1,GETDATE(), @active)

		SET @NewSTudyID= @@IDENTITY

		INSERT INTO FormGroupPeriods
		SELECT FormGroupId,PeriodId, [Order], @NewSTudyID FROM FormGroupPeriods WHERE StudyId=-100

		INSERT INTO FormConfig
		SELECT FormId,FormName,PeriodId,FormGroupId,@NewSTudyID,[Order],FormType,[Rows],Display,[Disabled] FROM FormConfig WHERE StudyId=-100

		INSERT INTO FormFields
		SELECT FormId,FieldId,FieldLiteral,FieldExportName,PeriodId,FormGroupId,@NewSTudyID,FieldDataType,FieldType,FieldMandatory,[Order],Display,[Disabled],Unit,LOVId
		FROM FormFields WHERE StudyId=-100

		INSERT INTO PrefilledFields
		SELECT FormId,FieldId,PeriodId,FormGroupId,@NewSTudyID,RowIndex,FieldValue FROM PrefilledFields WHERE StudyId=-100

		SET @result=0
	END
	ELSE
	BEGIN
		SET @result=1
	END
GO
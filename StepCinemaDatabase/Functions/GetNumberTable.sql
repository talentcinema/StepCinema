CREATE FUNCTION [dbo].[GetNumberTable]
(
	@number INT
)
RETURNS @returntable TABLE
(
	[id] INT
)
AS
BEGIN
	DECLARE @i INT;
	SET @i = 0;
	WHILE @i < @number
	BEGIN
		SET @i = @i + 1;
		INSERT INTO @returntable([id]) VALUES(@i);
	END
	RETURN;
END

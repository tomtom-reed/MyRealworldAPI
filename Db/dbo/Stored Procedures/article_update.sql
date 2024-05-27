CREATE PROCEDURE [dbo].[article_update]
	@slug		VARCHAR (50),
	@authorId	INT,
	@title		VARCHAR (50) null,
	@description	VARCHAR (50) null,
	@body		TEXT null
AS
	IF (NOT EXISTS(SELECT 1 FROM Articles WHERE slug = @slug AND authorId = @authorId))
		THROW 50000, 'Article not found', 1;
	IF (@title IS NULL AND @description IS NULL AND @body IS NULL)
		THROW 50000, 'No fields to update', 1;
	DECLARE @QUERY1 AS NVARCHAR(3000)
	SET @QUERY1 = N'UPDATE Articles SET '
	IF @title IS NOT NULL
	BEGIN
		SET @QUERY1 = @QUERY1 + N'title = @title, '
	END
	IF @description IS NOT NULL
	BEGIN
		SET @QUERY1 = @QUERY1 + N'description = @description, '
	END
	IF @body IS NOT NULL
	BEGIN
		SET @QUERY1 = @QUERY1 + N'body = @body, '
	END
	SET @QUERY1 = @QUERY1 + N'updatedAt = GETUTCDATE() WHERE slug = @slug'
	DECLARE @ParamDefinition AS NVarchar(2000)
	SET @ParamDefinition = N' @title VARCHAR(50) NULL,
				@description VARCHAR(50) NULL,
				@body TEXT NULL'
	EXEC sp_executesql @QUERY1, @ParamDefinition, @title, @description, @body;
	RETURN;

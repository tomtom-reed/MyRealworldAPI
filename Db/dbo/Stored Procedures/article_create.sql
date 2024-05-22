CREATE PROCEDURE [dbo].[article_create]
	@slug        VARCHAR (50),
	@title       VARCHAR (50),
	@description VARCHAR (50),
	@body        TEXT,
	@StatusMsg VARCHAR(10) OUTPUT

AS
	-- COPILOT HERE
	IF (EXISTS(SELECT 1 FROM Articles WHERE slug = @slug))
	BEGIN
		SELECT @StatusMsg = 'Collision';
		RETURN;
	END

	INSERT INTO Articles 
		(slug, 
		title, 
		description, 
		body,
		createdAt,
		updatedAt)
	VALUES
		(@slug,
		@title,
		@description,
		@body,
		GETUTCDATE(),
		GETUTCDATE())
	RETURN;
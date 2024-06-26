CREATE PROCEDURE [dbo].[article_create]
	@slug        CHAR (44),
	@title       VARCHAR (140),
	@description VARCHAR (400),
	@body        TEXT,
	-- multiple tags, done using comma delimited 
	@tags        VARCHAR (MAX) = NULL,
	-- author
	@userId      INT
AS
	-- COPILOT HERE
	IF (EXISTS(SELECT 1 FROM Articles WHERE slug = @slug))
		THROW 50000, 'Collision', 1;

	BEGIN TRANSACTION;
	BEGIN TRY;
		INSERT INTO Articles 
			(slug, 
			title, 
			description, 
			body,
			authorId,
			createdAt,
			updatedAt)
		VALUES
			(@slug,
			@title,
			@description,
			@body,
			@userId,
			GETUTCDATE(),
			GETUTCDATE())

		IF @@ROWCOUNT = 0
			THROW 50000, 'Failed to create Article', 1;
		INSERT INTO Tags (slug, tag) SELECT @slug, value FROM STRING_SPLIT(@tags, ',')
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH;
	RETURN;
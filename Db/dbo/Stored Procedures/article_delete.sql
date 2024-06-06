CREATE PROCEDURE [dbo].[article_delete]
	@slug VARCHAR (50),
	@author_id INT
AS
	BEGIN
		-- Delete article
		DELETE FROM Articles WHERE slug = @slug and authorId = @author_id
		IF @@ROWCOUNT = 0
		BEGIN
			RAISERROR('Article not found', 16, 1)
			RETURN
		END
		DELETE FROM Tags WHERE slug = @slug
	END
	RETURN
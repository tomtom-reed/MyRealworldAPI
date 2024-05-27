CREATE PROCEDURE [dbo].[article_delete]
	@slug VARCHAR (50),
	@StatusMsg VARCHAR(10) OUTPUT
AS
	DELETE FROM Tags WHERE slug = @slug
	DELETE FROM Articles WHERE slug = @slug
	IF @@ROWCOUNT = 0
		SET @StatusMsg = 'No Update';
	RETURN
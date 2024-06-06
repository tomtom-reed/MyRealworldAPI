CREATE PROCEDURE [dbo].[favorite_delete]
	@userId INT,
	@slug VARCHAR(50)
AS
	DELETE FROM Favorites WHERE userId = @userId AND favoritedSlug = @slug
	RETURN

CREATE PROCEDURE [dbo].[favorite_delete]
	@userId INT,
	@slug CHAR(44)
AS
	DELETE FROM Favorites WHERE userId = @userId AND favoritedSlug = @slug
	RETURN

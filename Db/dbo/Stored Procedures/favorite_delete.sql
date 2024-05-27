CREATE PROCEDURE [dbo].[favorite_delete]
	@userId INT NOT NULL,
	@slug VARCHAR(50) NOT NULL
AS
	DELETE FROM Favorites WHERE userId = @userId AND favoritedSlug = @slug
	RETURN

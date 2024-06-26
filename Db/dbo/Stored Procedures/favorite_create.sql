CREATE PROCEDURE [dbo].[favorite_create]
	@userId int,
	@slug char(44)
AS
	INSERT INTO Favorites (userId, favoritedSlug) VALUES (@userId, @slug)
	RETURN

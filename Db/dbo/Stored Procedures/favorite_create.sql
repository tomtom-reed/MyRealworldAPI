CREATE PROCEDURE [dbo].[favorite_create]
	@userId int,
	@slug varchar(50)
AS
	INSERT INTO Favorites (userId, favoritedSlug) VALUES (@userId, @slug)
	RETURN

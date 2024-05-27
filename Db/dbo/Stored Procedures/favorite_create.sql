CREATE PROCEDURE [dbo].[favorite_create]
	@userId int not null,
	@slug varchar(50) not null
AS
	INSERT INTO Favorites (userId, favoritedSlug) VALUES (@userid, @slug)
	RETURN

CREATE PROCEDURE [dbo].[profile_follow]
	@followerId int not null,
	@followedUsername varchar(50) not null
AS
	-- COPILOT : GENERATED CODE
	DECLARE @followedId int
	SELECT @followedId = id FROM [dbo].[Users] WHERE username = @followedUsername -- copilot failed here
	IF @followedId IS NULL
		THROW 50000, 'User does not exist', 1
	IF @followerId = @followedId
		THROW 50000, 'Cannot follow yourself', 1
	IF EXISTS (SELECT * FROM [dbo].[Follows] WHERE userId = @followerId AND followedUserId = @followedId) -- note, copilot does not like plural nouns
		THROW 50000, 'Already following', 1
	INSERT INTO [dbo].[Follows] (userId, followedUserId) VALUES (@followerId, @followedId)
	RETURN 0

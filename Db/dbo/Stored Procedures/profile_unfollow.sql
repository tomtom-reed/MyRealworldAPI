CREATE PROCEDURE [dbo].[profile_unfollow]
	@followerId int,
	@followedUsername varchar(50)
AS
	DECLARE @followedId int
	SELECT @followedId = Id FROM [dbo].[Users] WHERE username = @followedUsername
	IF @followedId IS NULL
		THROW 50000, 'User does not exist', 1
	DELETE FROM Follows where userId = @followerId and followedUserId = @followedId 
	-- doesn't matter if there's anything to update, the delete will just do nothing
	-- yes, yes, thank you copilot for finishing my sentences. And again. And again.
	-- if only you'd done anything else here, I'd be able to say you were useful. Yes thank you for that sentence too.
	RETURN 0

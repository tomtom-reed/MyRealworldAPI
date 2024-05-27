CREATE PROCEDURE [dbo].[profile_get]
	@username varchar(50) not null,
	@followerId int null
AS
	SELECT 
		u.username as username,
		u.bio as bio,
		u.img as img
	FROM [dbo].[Users] u
	WHERE u.username = @username

	IF @followerId IS NOT NULL
		SELECT 
			CASE WHEN EXISTS (SELECT * FROM [dbo].[Follows] WHERE userId = @followerId AND followedUserId = u.id) THEN 1 ELSE 0 END as following
	RETURN;
GO

-- Returns two result sets that need to be handled separately in the DA
CREATE PROCEDURE [dbo].[profile_get]
	@username varchar(50),
	@followerId int null
AS
	SELECT 
		u.username as username,
		u.bio as bio,
		u.img as img,
		CASE WHEN @followerId IS NOT NULL THEN 
			CASE WHEN EXISTS (SELECT 1 FROM [dbo].[Follows] WHERE userId = @followerId AND followedUserId = U.Id) THEN 1 ELSE 0 END
		ELSE
			0
		END as following
	FROM [dbo].[Users] u
	WHERE u.username = @username
	RETURN;
GO

-- Returns two result sets that need to be handled separately in the DA
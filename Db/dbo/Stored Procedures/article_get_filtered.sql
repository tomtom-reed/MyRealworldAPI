-- The logic in this one might be a bit too heavy for a stored procedure
CREATE PROCEDURE [dbo].[article_get_filtered]
	@offset int null,
	@limit int null,
	@author varchar(50) null, -- Username, not ID
	@followerId int null,
	@followerName varchar(50) null, -- Username
	@tags varchar(MAX) null, -- comma delimited. Inclusive/any. 
	@slug varchar(50) null
AS
	If @followerName IS NOT NULL
	BEGIN
		SELECT @followerId = Id FROM [dbo].[Users] WHERE username = @followerName
	END
	SELECT
		A.slug as slug,
		A.title as title,
		A.description as description,
		A.body as body,
		A.createdAt as createdAt,
		A.updatedAt as updatedAt,
		CASE WHEN @followerId IS NOT NULL THEN 
			CASE WHEN EXISTS (SELECT 1 FROM [dbo].[Favorites] WHERE userId = @followerId AND favoritedSlug = A.slug) THEN 1 ELSE 0 END
		ELSE
			0
		END as favorited,
		(SELECT COUNT(*) FROM [dbo].[Favorites] WHERE favoritedSlug = A.slug) as favoritesCount,
		U.username as authorUsername,
		U.bio as authorBio,
		U.img as authorImg,
		CASE WHEN @followerId IS NOT NULL THEN 
			CASE WHEN EXISTS (SELECT 1 FROM [dbo].[Follows] WHERE userId = @followerId AND followedUserId = A.authorId) THEN 1 ELSE 0 END
		ELSE
			0
		END as following,
		(SELECT STRING_AGG(tag, ',') FROM [dbo].[Tags] WHERE slug = A.slug) as tagList
	FROM Articles A 
	INNER JOIN Users U ON A.authorId = U.Id
	WHERE 
		(@slug IS NULL OR A.slug = @slug)
		AND (@author IS NULL OR U.username = @author)
		AND (@followerId IS NULL OR EXISTS (SELECT 1 FROM [dbo].[Follows] WHERE userId = @followerId AND followedUserId = A.authorId))
		AND (@tags IS NULL OR EXISTS (SELECT 1 FROM [dbo].[Tags] WHERE slug = A.slug AND tag IN (SELECT value FROM STRING_SPLIT(@tags, ','))))
	ORDER BY A.createdAt DESC
	OFFSET ISNULL(@offset, 0) ROWS
	FETCH NEXT ISNULL(@limit, 20) ROWS ONLY
	RETURN;
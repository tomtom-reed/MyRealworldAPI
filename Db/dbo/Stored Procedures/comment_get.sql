CREATE PROCEDURE [dbo].[comment_get]
	@slug varchar(50) not null,
	@commentId int null,
	@userid int null
AS
	SELECT 
		AC.Id as id,
		c.body as body,
		c.createdAt as createdAt,
		c.updatedAt as updatedAt,
		u.username as username,
		u.bio as bio,
		u.img as img,
		CASE WHEN @userid IS NOT NULL THEN 
			CASE WHEN EXISTS (SELECT 1 FROM [dbo].[Follows] WHERE userId = @userid AND followedUserId = U.Id) THEN 1 ELSE 0 END
		ELSE
			0
		END as following
	FROM Articles A
		INNER JOIN Comments C ON A.slug = C.articleSlug
		INNER JOIN Users U ON C.authorId = U.id
		INNER JOIN ArticleComments AC ON A.slug = AC.articleSlug AND C.id = AC.commentId
	WHERE A.slug = @slug
		AND (@commentId IS NULL OR AC.Id = @commentId)
	ORDER BY AC.Id ASC

	RETURN;
GO
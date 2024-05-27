CREATE PROCEDURE [dbo].[comment_create]
	@authorId int not null,
	@articleSlug varchar(50) not null,
	@body text not null,
	@commentId int OUTPUT
AS
	BEGIN TRANSACTION;
	BEGIN TRY
		INSERT INTO Comments (authorId, articleSlug, body, createdAt, updatedAt) 
			VALUES (@authorId, @articleSlug, @body, GETUTCDATE(), GETUTCDATE());
		IF @@ROWCOUNT = 0
			THROW 50000, 'Failed to create Comment', 1;
		INSERT INTO ArticleComments (Id, articleSlug, commentId) 
			VALUES ((SELECT MAX(Id)+1 FROM ArticleComments WHERE articleSlug = @articleSlug and commentId = SCOPE_IDENTITY()), 
				@articleSlug, 
				SCOPE_IDENTITY());
		IF @@ROWCOUNT = 0
			THROW 50000, 'Failed to create ArticleComment', 1;
		COMMIT TRANSACTION;
	END TRY 
	BEGIN CATCH 
		ROLLBACK TRANSACTION;
		SELECT ERROR_MESSAGE() AS ErrorMessage;
		RETURN 1;
	END CATCH;
	SET @commentId = SCOPE_IDENTITY();
	RETURN 0;
GO

--So this one's actually a pain
--The comment ID is an integer, but it's a combination of the article slug and the comment ID.
--You can't just use commentId because it's not unique across articles.
--So we need to insert the comment, get the ID, and then insert the article-comment relationship using MAX()+1 as the ID in ArticleComments.
--Not count. Comments can be deleted, so we can't rely on the count of comments to be the ID.
--Thank you for coming to my TED talk (Copilot's fault)
CREATE PROCEDURE [dbo].[comment_create]
	@authorId int,
	@articleSlug char(44),
	@body text,
	@commentId int OUTPUT
AS
	BEGIN TRANSACTION;
	BEGIN TRY
		INSERT INTO Comments (authorId, articleSlug, body, createdAt, updatedAt) 
			VALUES (@authorId, @articleSlug, @body, GETUTCDATE(), GETUTCDATE());
		IF @@ROWCOUNT = 0
			THROW 50000, 'Failed to create Comment', 1;
		DECLARE @createdCommentId int;
		DECLARE @articleCommentId int;
		SET @createdCommentId = SCOPE_IDENTITY();
		SET @articleCommentId = ISNULL((SELECT MAX(Id) + 1 FROM ArticleComments WHERE articleSlug = @articleSlug), 0);
		INSERT INTO ArticleComments (Id, articleSlug, commentId) 
			VALUES (ISNULL(@articleCommentId, 0), @articleSlug, @createdCommentId);
		SELECT @commentId = @articleCommentId;
		IF @@ROWCOUNT = 0
			THROW 50000, 'Failed to create ArticleComment', 1;
		COMMIT TRANSACTION;
	END TRY 
	BEGIN CATCH 
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH;
GO

--So this one's actually a pain
--The comment ID is an integer, but it's a combination of the article slug and the comment ID.
--You can't just use commentId because it's not unique across articles.
--So we need to insert the comment, get the ID, and then insert the article-comment relationship using MAX()+1 as the ID in ArticleComments.
--Not count. Comments can be deleted, so we can't rely on the count of comments to be the ID.
--And we can't just mark them as deleted because PII burden and probably GDPR or something.
--Thank you for coming to my TED talk (Copilot's fault)
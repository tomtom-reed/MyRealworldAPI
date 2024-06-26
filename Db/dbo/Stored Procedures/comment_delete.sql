CREATE PROCEDURE [dbo].[comment_delete]
	@articleSlug char(44),
	@commentId int,
	@authorId int
AS
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @deleted TABLE (
			Id INT
		);
		DELETE FROM Comments OUTPUT DELETED.Id INTO @deleted WHERE Id = @commentId AND authorId = @authorId;
		DECLARE @deletedId INT;
		SET @deletedId = (SELECT TOP 1 Id FROM @deleted);
		IF @deletedId IS NULL
			THROW 50000, 'Comment does not exist or you are not the author', 1;
		DELETE FROM ArticleComments WHERE commentId = @deletedId AND articleSlug = @articleSlug;
		COMMIT TRANSACTION;
	END TRY 
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SELECT ERROR_MESSAGE() AS ErrorMessage;
		RETURN 1;
	END CATCH;
RETURN 0

CREATE PROCEDURE [dbo].[comment_delete]
	@articleSlug varchar(50) not null,
	@commentId int not null,
	@authorId int not null
AS
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @deletedId int;
		DELETE FROM Comments OUTPUT DELETED.id INTO @deletedId WHERE id = @commentId AND authorId = @authorId;
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

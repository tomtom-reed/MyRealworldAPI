CREATE TABLE [dbo].[ArticleComments]
(
	[Id] INT NOT NULL,
	[articleSlug] VARCHAR(44) NOT NULL,
	[commentId] INT NOT NULL,
	UNIQUE(Id, articleSlug, commentId),
)

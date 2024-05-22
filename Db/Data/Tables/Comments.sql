CREATE TABLE [dbo].[Comments]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [articleId] INT NOT NULL, 
    [authorId] INT NOT NULL,
    [body] TEXT NOT NULL, 
    [createdAt] DATETIME NOT NULL, 
    [updatedAt] DATETIME NULL
)

--commentId
--		articleId
--		createdAt
--		updatedAt
--		body
--		authorID 
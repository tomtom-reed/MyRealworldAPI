CREATE TABLE [dbo].[Favorites]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [favoritedSlug] VARCHAR(50) NOT NULL, 
    CONSTRAINT [favoriting_user] FOREIGN KEY ([userId]) REFERENCES [Users]([Id]), 
    CONSTRAINT [favorited_article] FOREIGN KEY ([favoritedSlug]) REFERENCES [Articles]([slug])
)

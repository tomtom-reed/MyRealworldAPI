CREATE TABLE [dbo].[Favorites] (
    [userId]        INT          NOT NULL,
    [favoritedSlug] CHAR (44) NOT NULL,
    PRIMARY KEY CLUSTERED ([userId] ASC),
    CONSTRAINT [favorited_article] FOREIGN KEY ([favoritedSlug]) REFERENCES [dbo].[Articles] ([slug]),
    CONSTRAINT [favoriting_user] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([Id])
);


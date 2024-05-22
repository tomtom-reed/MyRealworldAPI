CREATE TABLE [dbo].[Follows] (
    [userId]         INT NOT NULL,
    [followedUserId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([userId] ASC),
    CONSTRAINT [followed_key] FOREIGN KEY ([followedUserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [follower_key] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([Id])
);


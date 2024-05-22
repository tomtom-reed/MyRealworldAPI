CREATE TABLE [dbo].[Follows]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [followedUserId] INT NOT NULL, 
    CONSTRAINT [follower_key] FOREIGN KEY ([userId]) REFERENCES [Users]([Id]), 
    CONSTRAINT [followed_key] FOREIGN KEY ([followedUserId]) REFERENCES [Users]([Id])
)


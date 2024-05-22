CREATE TABLE [dbo].[Comments] (
    [Id]        INT      NOT NULL,
    [articleId] INT      NOT NULL,
    [authorId]  INT      NOT NULL,
    [body]      TEXT     NOT NULL,
    [createdAt] DATETIME NOT NULL,
    [updatedAt] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


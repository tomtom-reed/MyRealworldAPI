CREATE TABLE [dbo].[Comments] (
    [Id]        INT      IDENTITY(1, 1) NOT NULL,
    [articleSlug] VARCHAR(50)      NOT NULL,
    [authorId]  INT      NOT NULL,
    [body]      TEXT     NOT NULL,
    [createdAt] DATETIME NOT NULL,
    [updatedAt] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


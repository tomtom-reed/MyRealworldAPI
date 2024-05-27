CREATE TABLE [dbo].[Articles] (
    [slug]        VARCHAR (50) NOT NULL,
    [title]       VARCHAR (50) NULL,
    [description] VARCHAR (50) NULL,
    [body]        TEXT         NULL,
    [authorId]    INT          NULL,
    [createdAt]   DATETIME     NULL,
    [updatedAt]   DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([slug] ASC)
);


CREATE TABLE [dbo].[Articles] (
    [slug]        CHAR(44) NOT NULL,
    [title]       VARCHAR (140) NULL,
    [description] VARCHAR(400) NULL,
    [body]        TEXT         NULL,
    [authorId]    INT          NULL,
    [createdAt]   DATETIME     NULL,
    [updatedAt]   DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([slug] ASC)
);


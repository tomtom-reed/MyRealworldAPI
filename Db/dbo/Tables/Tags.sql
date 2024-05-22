CREATE TABLE [dbo].[Tags] (
    [slug] VARCHAR (50) NOT NULL,
    [tag]  VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([slug] ASC),
    CONSTRAINT [article_slug] FOREIGN KEY ([slug]) REFERENCES [dbo].[Articles] ([slug])
);


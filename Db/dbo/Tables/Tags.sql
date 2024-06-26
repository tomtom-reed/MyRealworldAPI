CREATE TABLE [dbo].[Tags] (
    [slug] CHAR (44) NOT NULL,
    [tag]  VARCHAR (30) NOT NULL,
    CONSTRAINT [article_slug] FOREIGN KEY ([slug]) REFERENCES [dbo].[Articles] ([slug])
);

GO

CREATE INDEX [slugindex] ON [dbo].[Tags] ([slug])
GO
CREATE INDEX [tagindex] on [dbo].[Tags] ([tag])
GO

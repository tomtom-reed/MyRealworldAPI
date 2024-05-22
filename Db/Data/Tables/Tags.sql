CREATE TABLE [dbo].[Tags]
(
	[slug] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [tag] VARCHAR(50) NOT NULL, 
    CONSTRAINT [article_slug] FOREIGN KEY ([slug]) REFERENCES [Articles]([Slug])
)

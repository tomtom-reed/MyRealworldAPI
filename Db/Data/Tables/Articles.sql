CREATE TABLE [dbo].[Articles]
(
	[slug] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [title] VARCHAR(50) NULL, 
    [description] VARCHAR(50) NULL, 
    [body] TEXT NULL, 
    [createdAt] DATETIME NULL, 
    [updatedAt] DATETIME NULL
)

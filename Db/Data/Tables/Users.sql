CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [username] VARCHAR(50) NOT NULL, 
    [email_hash] NCHAR(10) NOT NULL, 
    [email_crypt] NCHAR(10) NOT NULL, 
    [password] NCHAR(10) NOT NULL, 
    [salt] NCHAR(10) NOT NULL, 
    [bio] NCHAR(10) NULL, 
    [image] NCHAR(10) NULL, 
    [createdAt] DATETIME NOT NULL, 
    [updatedAt] DATETIME NOT NULL, 
    [lastLogin] DATETIME NULL 
)

GO

CREATE INDEX [email] ON [dbo].[Users] ([email_hash])

GO

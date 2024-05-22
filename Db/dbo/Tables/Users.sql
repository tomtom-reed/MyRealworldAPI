CREATE TABLE [dbo].[Users] (
    [Id]          INT IDENTITY(1,1)   NOT NULL,
    [username]    VARCHAR (50) NOT NULL,
    [email_hash]  BINARY(32)   NOT NULL UNIQUE,
    [email_crypt] VARBINARY(MAX)   NOT NULL,
    [pwd]    BINARY(80)   NOT NULL,
    [bio]         TEXT   NULL,
    [img]       VARCHAR(256)   NULL,
    [createdAt]   DATETIME     NOT NULL,
    [updatedAt]   DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [email]
    ON [dbo].[Users]([email_hash] ASC);


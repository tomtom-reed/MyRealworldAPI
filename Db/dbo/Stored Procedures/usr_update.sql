-- https://www.codeproject.com/Articles/20815/Building-Dynamic-SQL-In-a-Stored-Procedure
-- https://learn.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-executesql-transact-sql?view=sql-server-ver16

CREATE PROCEDURE [dbo].[usr_update]
	@userid int,
	@new_email_hash BINARY(32) null,
    @new_email_crypt VARBINARY(MAX) null,
	@new_username VARCHAR(50) null,
	@new_password BINARY(96) null,
	@new_bio TEXT null,
	@new_image VARCHAR(256) null,
	@StatusMsg VARCHAR(10) OUTPUT
AS
	IF NOT EXISTS(SELECT 1 FROM Users WHERE Id = @userid)
	BEGIN
		SET @StatusMsg = 'Miss';
		RETURN;
	END

	IF @new_email_crypt IS NULL 
		AND @new_email_hash IS NULL
		AND @new_username IS NULL
		AND @new_password IS NULL
		AND @new_bio IS NULL
		AND @new_image IS NULL
	BEGIN
		SET @StatusMsg = 'No Update';
		RETURN
	END

	DECLARE @QUERY1 AS NVARCHAR(3000)

	SET @QUERY1 = N'UPDATE Users SET '

	IF @new_email_crypt IS NOT NULL AND @new_email_hash IS NOT NULL
	BEGIN
		SET @QUERY1 = @QUERY1 + N'email_crypt = @email_crypt, '
		SET @QUERY1 = @QUERY1 + N'email_hash = @email_hash, '
	END

	IF @new_username IS NOT NULL
	BEGIN 
		SET @QUERY1 = @QUERY1 + N'username = @username, '
	END 

	IF @new_bio IS NOT NULL
	BEGIN 
		SET @QUERY1 = @QUERY1 + N'bio = @bio, '
	END 

	IF @new_image IS NOT NULL
	BEGIN
		SET @QUERY1 = @QUERY1 + N'img = @image, '
	END 
	
	DECLARE @ParamDefinition AS NVarchar(2000)
	SET @ParamDefinition = N' @email_crypt VARBINARY(MAX) NULL,
                @email_hash BINARY(32) NULL,
                @username VARCHAR(50) NULL,
                @password BINARY(96) NULL,
                @bio TEXT NULL,
                @image VARCHAR(256) NULL,
				@userId int'
    /* Execute the Transact-SQL String with all parameter value's 
       Using sp_executesql Command */
	SET @QUERY1 = @QUERY1 + N'updatedAt = GETUTCDATE() WHERE Id = @userId'
	PRINT @QUERY1
	PRINT @ParamDefinition
	EXECUTE sp_executesql
		@QUERY1,
		@ParamDefinition,
		@new_email_crypt,
		@new_email_hash,
		@new_username,
		@new_password,
		@new_bio,
		@new_image,
		@userid

	IF @@ROWCOUNT = 0
		SET @StatusMsg = 'No Update';
	RETURN
    -- @@ERROR <> 0 GoTo ErrorHandler
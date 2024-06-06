CREATE PROCEDURE [dbo].[usr_create]
	@username VARCHAR(50),
	@emailhash BINARY(64),
	@emailcrypt VARBINARY(MAX),
	@password BINARY(80)
AS
	--IF (EXISTS(SELECT 1 FROM USERS WHERE username = @username))
	--BEGIN
	--	SELECT @StatusMsg = 'Collision';
	--	RETURN;
	--END

	INSERT INTO Users 
		(username, 
		email_hash, 
		email_crypt, 
		pwd,
		createdAt,
		updatedAt)
	VALUES
		(@username,
		@emailhash, 
		@emailcrypt,
		@password,
		GETUTCDATE(),
		GETUTCDATE())

	SELECT SCOPE_IDENTITY(); -- call with (Int32) cmd.ExecuteScalar()
	RETURN;
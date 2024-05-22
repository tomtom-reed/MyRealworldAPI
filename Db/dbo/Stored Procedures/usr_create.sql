CREATE PROCEDURE [dbo].[usr_create]
	@username VARCHAR(50),
	@emailhash BINARY(64),
	@emailcrypt VARBINARY(MAX),
	@password BINARY(80),
	@StatusMsg VARCHAR(10) OUTPUT
AS
	IF (EXISTS(SELECT 1 FROM USERS WHERE username = @username))
	BEGIN
		SELECT @StatusMsg = 'Collision';
		RETURN;
	END

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
	RETURN;


-- username, emailhash, emailcrypt, password
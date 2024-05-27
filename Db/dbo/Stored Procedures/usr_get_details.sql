CREATE PROCEDURE [dbo].[usr_get_details]
	@email varchar(50) null,
	@username varchar(50) null,
	@userid int null
AS
	IF @email is null AND @username is null AND @userid is null
		RETURN -1;
	SELECT 
		Id, 
		username, 
		email_crypt, 
		pwd, 
		bio, 
		img,
		createdAt,
		updatedAt
	FROM Users 
	WHERE (@email IS NULL OR email_hash = @email) 
		AND (@username IS NULL OR username = @username) 
		AND (@userid IS NULL OR Id = @userid)
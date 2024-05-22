CREATE PROCEDURE [dbo].[usr_get_details]
	@email varchar(50)
AS
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
	WHERE email_hash = @email;
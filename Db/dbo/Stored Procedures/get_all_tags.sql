CREATE PROCEDURE [dbo].[get_all_tags]
AS
	SELECT DISTINCT tag FROM [dbo].[Tags];
	RETURN;
GO

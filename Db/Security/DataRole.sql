CREATE ROLE [DataRole]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [DataRole] ADD MEMBER [qadb];


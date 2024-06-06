using Contracts.Communicator.Response;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace RealworldWebHost.DataAccess
{

    public interface IProfileDA
    {
        ProfileGetResponseContract GetProfile(string username, int? followerId);
        bool Follow(int userId, string username);
        bool StopFollowing(int userId, string username);
        bool SetFavorite(int userId, string slug);
        bool DeleteFavorite(int userId, string slug);
    }
    public class ProfileDA : IProfileDA
    {
        private const string PROC_PROFILE_GET = "profile_get";
        private const string PROC_PROFILE_FOLLOW = "profile_follow";
        private const string PROC_PROFILE_UNFOLLOW = "profile_unfollow";
        private const string PROC_FAVORITE_CREATE = "favorite_create";
        private const string PROC_FAVORITE_DELETE = "favorite_delete";

        private string connectionString;

        public ProfileDA(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ProfileGetResponseContract GetProfile(string username, int? followerId)
        {
            ProfileGetResponseContract profile = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_PROFILE_GET, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@username", username));
                        cmd.Parameters.Add(new SqlParameter("@followerId", followerId));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                profile = new ProfileGetResponseContract();
                                profile.Username = Convert.ToString(reader["username"]) ?? "";
                                profile.Bio = Convert.ToString(reader["bio"]) ?? "";
                                profile.Image = Convert.ToString(reader["img"]) ?? "";
                                profile.Following = followerId != null ? Convert.ToBoolean(reader["following"]) : null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return profile;
        }

        public bool Follow(int userId, string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_PROFILE_FOLLOW, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@followerId", userId));
                        cmd.Parameters.Add(new SqlParameter("@followedUsername", username));
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            Console.WriteLine("Follow User DA failed");
                            return false ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool StopFollowing(int userId, string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_PROFILE_UNFOLLOW, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@followerId", userId));
                        cmd.Parameters.Add(new SqlParameter("@followedUsername", username));
                        cmd.ExecuteNonQuery();
                    }
                    // delete succeeds if it deletes what doesn't exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool SetFavorite(int userId, string slug)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_FAVORITE_CREATE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@userId", userId));
                        cmd.Parameters.Add(new SqlParameter("@slug", slug));
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            Console.WriteLine("SetFavorite failed");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool DeleteFavorite(int userId, string slug)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_FAVORITE_DELETE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@userId", userId));
                        cmd.Parameters.Add(new SqlParameter("@slug", slug));
                        cmd.ExecuteNonQuery();
                    }
                    // delete succeeds if it deletes what doesn't exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}

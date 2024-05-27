using Contracts.Communicator.Request;
using Contracts.Models;
using Microsoft.Data.SqlClient;
using RealworldWebHost.Models;
using RealworldWebHost.Utilities;
using System.Reflection.Metadata.Ecma335;

/*
 * The DA should be responsible for encrypting and decrypting.
 * The controller should be responsible for business logic, including password validation. 
 */

namespace RealworldWebHost.DataAccess
{

    public interface IUserDA
    {
        bool CreateUser(String username, string email, string password);
        UserDetails GetUserDetailsByEmail(string email);
        UserDetails GetUserDetailsByUsername(string username);
        UserDetails GetUserDetailsById(int id);
        bool UpdateUserDetails(UserUpdateContract req);
    }
    public class UserDA : IUserDA
    {
        private const string PROC_USR_CREATE = "usr_create";
        private const string PROC_USR_DETAILS = "usr_get_details";
        private const string PROC_USR_UPDATE = "usr_update";


        private string ConnectionString;
        private ISecUtils secUtils;
        public UserDA(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private void ExecuteNonQuery(String proc, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(proc, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return;
        }

        public bool CreateUser(String username, string email, string password)
        {
            byte[] emailhash = secUtils.HashSha256(email); //32
            byte[] emailcipher = secUtils.Encrypt(email); //32
            byte[] pwhash = secUtils.HashPassword(password); //80
            string? status = "Fail";
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_USR_CREATE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@username", username));
                        cmd.Parameters.Add(new SqlParameter("@emailhash", emailhash));
                        cmd.Parameters.Add(new SqlParameter("@emailcrypt", emailcipher));
                        cmd.Parameters.Add(new SqlParameter("@password", pwhash));
                        var _statusParam = new SqlParameter("@StatusMsg", System.Data.SqlDbType.VarChar, 10);
                        _statusParam.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(_statusParam);
                        Int32 rows = cmd.ExecuteNonQuery();
                        if (rows == 0) {
                            Console.WriteLine("CreateUser failed with status: " + _statusParam.Value.ToString());
                        }
                        status = _statusParam.Value.ToString();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (string.IsNullOrEmpty(status))
            {
                return true;
            }
            return false;
        }

        private UserDetails ReaderToUserDetails(SqlDataReader reader)
        {
            UserDetails usr = new UserDetails();
            usr.Id = Convert.ToInt32(reader["Id"]);

            string? un = Convert.ToString(reader["username"]);
            usr.UserName = un == null ? "" : un;

            byte[] emc = (byte[])reader["email_crypt"];
            usr.Email = emc == null ? "" : secUtils.Decrypt(emc);

            usr.PasswordHash = (byte[])reader["pwd"];

            string? bio = Convert.ToString(reader["bio"]);
            usr.Bio = bio == null ? "" : bio;

            string? img = Convert.ToString(reader["img"]);
            usr.Img = img == null ? "" : img;

            usr.CreatedAt = Convert.ToDateTime(reader["createdAt"]);

            usr.UpdatedAt = Convert.ToDateTime(reader["updatedAt"]);
            return usr;
        }

        public UserDetails GetUserDetailsByEmail(string email)
        {
            byte[] emailhash = secUtils.HashSha256(email);
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_USR_DETAILS, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@email", emailhash));
                        cmd.Parameters.Add(new SqlParameter("@username", null));
                        cmd.Parameters.Add(new SqlParameter("@userid", null));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return ReaderToUserDetails(reader);
                            }
                            reader.Close();
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public UserDetails GetUserDetailsByUsername(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_USR_DETAILS, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@email", null));
                        cmd.Parameters.Add(new SqlParameter("@username", username));
                        cmd.Parameters.Add(new SqlParameter("@userid", null));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return ReaderToUserDetails(reader);
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public UserDetails GetUserDetailsById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_USR_DETAILS, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@email", null));
                        cmd.Parameters.Add(new SqlParameter("@username", null));
                        cmd.Parameters.Add(new SqlParameter("@userid", id));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return ReaderToUserDetails(reader);
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public bool UpdateUserDetails(UserUpdateContract req)
        {

            byte[] emailhash; //32
            byte[] emailcipher; //32
            byte[] pwhash; //80
            if (req.Email != null)
            {
                emailhash = secUtils.HashSha256(req.Email);
                emailcipher = secUtils.Encrypt(req.Email);
            }
            if (req.Password != null)
            {
                pwhash = secUtils.HashPassword(req.Password); //80
            }
            string? status = "Fail";
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_USR_UPDATE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@currentemail_hash", secUtils.HashSha256(req.UserCurrentEmail)));
                        cmd.Parameters.Add(new SqlParameter("@new_email_hash", 
                            req.Email != null ? secUtils.HashSha256(req.Email) : null));
                        cmd.Parameters.Add(new SqlParameter("@new_email_crypt", 
                            req.Email != null ? secUtils.Encrypt(req.Email) : null));
                        cmd.Parameters.Add(new SqlParameter("@new_username", req.Username));
                        cmd.Parameters.Add(new SqlParameter("@new_password", 
                            req.Password != null ? secUtils.HashPassword(req.Password) : null));
                        cmd.Parameters.Add(new SqlParameter("@new_bio", req.Bio));
                        cmd.Parameters.Add(new SqlParameter("@new_image", req.Image));
                        var _statusParam = new SqlParameter("@StatusMsg", System.Data.SqlDbType.VarChar, 10);
                        _statusParam.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(_statusParam);

                        Int32 rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            Console.WriteLine("UpdateUser failed with status: " + _statusParam.Value.ToString());
                        }
                        status = _statusParam.Value.ToString();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (!string.IsNullOrEmpty(status))
            {
                Console.WriteLine("Update Failed with status: " + status);
                return false;
            }
            return true;
        }
    }
}

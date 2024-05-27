using Contracts.Communicator.Response;
using Microsoft.Data.SqlClient;

namespace RealworldWebHost.DataAccess
{

    public interface ICommentDA
    {
        int CreateComment(string slug, int authorId, string body);
        bool DeleteComment(string slug, int commentId, int authorId);

        List<CommentGetResponseContract> GetByArticle(string slug, int? followerId);
        CommentGetResponseContract GetById(string slug, int commentId, int? followerId);
    }
    public class CommentDA : ICommentDA
    {
        private const string PROC_COMMENT_CREATE = "comment_create";
        private const string PROC_COMMENT_DELETE = "comment_delete";
        private const string PROC_COMMENT_GET = "comment_get";

        private string connectionString;
        public CommentDA(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int CreateComment(string slug, int authorId, string body)
        {
            int commentId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_COMMENT_CREATE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@slug", slug));
                        cmd.Parameters.Add(new SqlParameter("@authorId", authorId));
                        cmd.Parameters.Add(new SqlParameter("@body", body));
                        var _commentId = new SqlParameter("@commentId", System.Data.SqlDbType.Int);
                        _commentId.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(_commentId);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            return -1;
                        }
                        commentId = _commentId.Value as int? ?? -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return commentId;
        }

        public bool DeleteComment(string slug, int commentId, int authorId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_COMMENT_DELETE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@slug", slug));
                        cmd.Parameters.Add(new SqlParameter("@commentId", commentId));
                        cmd.Parameters.Add(new SqlParameter("@authorId", authorId));
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            return false;
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public List<CommentGetResponseContract> GetByArticle(string slug, int? followerId)
        {
            List<CommentGetResponseContract> comments = new List<CommentGetResponseContract>();
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_COMMENT_GET, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@slug", slug));
                        cmd.Parameters.Add(new SqlParameter("@commentId", null));
                        cmd.Parameters.Add(new SqlParameter("@followerId", followerId));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comments.Add(ReaderToResponse(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return comments;
        }

        public CommentGetResponseContract GetById(string slug, int commentId, int? followerId)
        {
            {
                List<CommentGetResponseContract> comments = new List<CommentGetResponseContract>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(this.connectionString))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(PROC_COMMENT_GET, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                        {
                            cmd.Parameters.Add(new SqlParameter("@slug", slug));
                            cmd.Parameters.Add(new SqlParameter("@commentId", commentId));
                            cmd.Parameters.Add(new SqlParameter("@userid", followerId));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    return ReaderToResponse(reader);
                                }
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
        }

        private CommentGetResponseContract ReaderToResponse(SqlDataReader reader)
        {
            CommentGetResponseContract comment = new CommentGetResponseContract();
            comment.Id = Convert.ToInt32(reader["id"]);
            comment.Body = Convert.ToString(reader["body"]) ?? "";
            comment.CreatedAt = Convert.ToDateTime(reader["createdAt"]);
            comment.UpdatedAt = Convert.ToDateTime(reader["updatedAt"]);
            comment.Author.Username = Convert.ToString(reader["username"]) ?? "";
            comment.Author.Bio = Convert.ToString(reader["bio"]) ?? "";
            comment.Author.Image = Convert.ToString(reader["img"]) ?? "";
            comment.Author.Following = Convert.ToBoolean(reader["following"]);
            return comment;
        }
    }
}

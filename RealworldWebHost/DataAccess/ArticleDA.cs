using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using Microsoft.Data.SqlClient;
using RealworldWebHost.Utilities;
using System.Linq.Expressions;

namespace RealworldWebHost.DataAccess
{
    // Handles data access for articles, including Tags

    public interface IArticleDA
    {
        /// <summary>
        /// Creates an article from the given contract. 
        /// Returns the slug of the article if successful, otherwise an empty string.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        string CreateArticle(ArticleCreateContract article);

        /// <summary>
        /// Updates an article. All fields are optional except for the slug.
        /// Returns true if successful or if no fields are provided for update. 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        bool UpdateArticle(ArticleUpdateContract article);

        bool DeleteArticle(ArticleDeleteContract article);

        /// <summary>
        /// Gets a list of articles filtered by any field except slug. All fields are optional. 
        /// FollowerName supercedes FollowerId if provided. 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        List<ArticleGetResponseContract> GetArticlesFiltered(ArticleGetContract article);

        /// <summary>
        /// Gets a single article with the provided slug. Optionally determines favorites and follows by followerId.
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="followerId"></param>
        /// <returns></returns>
        ArticleGetResponseContract GetArticle(string slug, int? followerId=null);

        List<string> GetAllTags();
    }
    public class ArticleDA : IArticleDA
    {
        private const string PROC_ARTICLE_CREATE = "article_create";
        private const string PROC_ARTICLE_UPDATE = "article_update";
        private const string PROC_ARTICLE_DELETE = "article_delete";
        private const string PROC_ARTICLE_GET_FILTERED = "article_get_filtered";
        private const string PROC_GET_TAGS = "get_all_tags";

        private ISecUtils secUtils;
        private string connectionString;
        public ArticleDA(ISecUtils secUtils, string connectionString)
        {
            this.secUtils = secUtils;
            this.connectionString = connectionString;
        }

        public string CreateArticle(ArticleCreateContract article)
        {
            // Slug is based on title but should be unique
            // We mock that by hashing the title and adding a timestamp
            // Then we represent the hash as a Base64 string (not UTF-8) to make it URL safe
            string slug = Convert.ToBase64String(secUtils.HashSha256(article.Title + DateTime.UtcNow.ToString()));
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_ARTICLE_CREATE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@slug", slug); // thank you copilot 
                        cmd.Parameters.AddWithValue("@author_id", article.AuthorId);
                        cmd.Parameters.AddWithValue("@title", article.Title);
                        cmd.Parameters.AddWithValue("@description", article.Description);
                        cmd.Parameters.AddWithValue("@body", article.Body);
                        cmd.Parameters.AddWithValue("@tags", string.Join(',', article.Tags));
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            return "";
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
            return slug;
        }

        public bool UpdateArticle(ArticleUpdateContract article)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_ARTICLE_UPDATE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@slug", article.Slug);
                        cmd.Parameters.AddWithValue("@author_id", article.AuthorId);
                        cmd.Parameters.AddWithValue("@title", article.Title);
                        cmd.Parameters.AddWithValue("@description", article.Description);
                        cmd.Parameters.AddWithValue("@body", article.Body);
                        cmd.Parameters.AddWithValue("@tags", article.Tags != null ? string.Join(',', article.Tags) : null);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
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

        public bool DeleteArticle(ArticleDeleteContract article)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_ARTICLE_DELETE, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@slug", article.Slug);
                        cmd.Parameters.AddWithValue("@author_id", article.AuthorId);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
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

        public ArticleGetResponseContract GetArticle(string slug, int? followerId = null)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_ARTICLE_GET_FILTERED, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@slug", slug);
                        cmd.Parameters.AddWithValue("@follower", followerId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return GetArticleContractFromReader(reader);
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public List<ArticleGetResponseContract> GetArticlesFiltered(ArticleGetContract article)
        {
            List<ArticleGetResponseContract> response = new List<ArticleGetResponseContract>();
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_ARTICLE_GET_FILTERED, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@offset", article.Offset);
                        cmd.Parameters.AddWithValue("@limit", article.Limit);
                        cmd.Parameters.AddWithValue("@author", article.Authorname);
                        cmd.Parameters.AddWithValue("@followerId", article.FollowedById);
                        cmd.Parameters.AddWithValue("@followerName", article.FollowedByName);
                        cmd.Parameters.AddWithValue("@tags", article.Tags != null ? string.Join(',', article.Tags) : null);
                        // slug is not valid for GetMany
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.Add(GetArticleContractFromReader(reader));
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        private ArticleGetResponseContract GetArticleContractFromReader(SqlDataReader reader)
        {
            ArticleGetResponseContract response = new ArticleGetResponseContract();
            response.Slug = Convert.ToString(reader["slug"]) ?? "";
            response.Title = Convert.ToString(reader["title"]) ?? "";
            response.Description = Convert.ToString(reader["description"]) ?? "";
            response.Body = Convert.ToString(reader["body"]) ?? "";
            response.CreatedAt = Convert.ToDateTime(reader["created_at"]);
            response.UpdatedAt = Convert.ToDateTime(reader["updated_at"]);
            response.Favorited = Convert.ToBoolean(reader["favorited"]);
            response.FavoritesCount = Convert.ToInt32(reader["favorites_count"]);
            response.Author.Username = Convert.ToString(reader["authorUsername"]) ?? "";
            response.Author.Bio = Convert.ToString(reader["authorBio"]) ?? "";
            response.Author.Image = Convert.ToString(reader["authorImg"]) ?? "";
            response.Author.Following = Convert.ToBoolean(reader["following"]);
            response.Tags = (Convert.ToString(reader["tags"]) ?? "").Split(',').ToList();
            return response;
        }

        public List<string> GetAllTags()
        {
            List<string> response = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(PROC_GET_TAGS, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.Add(Convert.ToString(reader["tag"]) ?? "");
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return response;
        }
    }
}

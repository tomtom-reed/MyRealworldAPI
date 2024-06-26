using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using RealworldWeb.Utils;
using RestSharp;

namespace RealworldWeb.Caller
{

    public interface IArticleCaller
    {
        Task<List<ArticleGetResponseContract>> GetArticlesByFeed(ArticleGetContract request); // aka follow
        Task<List<ArticleGetResponseContract>> GetArticlesFiltered(ArticleGetContract request);
        Task<ArticleGetResponseContract> CreateArticle(ArticleCreateContract request);
        Task<ArticleGetResponseContract> GetArticle(string slug, int? userid);
        Task<ArticleGetResponseContract> UpdateArticle(ArticleUpdateContract request);
        Task<bool> DeleteArticle(ArticleDeleteContract contract);
    }

    public class ArticleCaller : IArticleCaller
    {
        RestClient client;

        public ArticleCaller(WebConfiguration config)
        {
            var options = new RestClientOptions(config.Connections_WebHost);
            this.client = new RestClient(options);
        }

        public async Task<ArticleGetResponseContract> CreateArticle(ArticleCreateContract request)
        {
            var req = new RestRequest("api/article/create");
            var body = new ArticleCreateRequest();
            body.Article = request;
            req.AddBody(body);

            var res = await client.PostAsync<ArticleCreateResponse>(req);

            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                Console.WriteLine("Create Article Success");
                return res.Article;
            }
            Console.WriteLine("Call to Create Article failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            return null;
        }

        public async Task<ArticleGetResponseContract> UpdateArticle(ArticleUpdateContract request)
        {
            var req = new RestRequest("api/article/create");
            var body = new ArticleUpdateRequest();
            body.Article = request;
            req.AddBody(body);

            var res = await client.PostAsync<ArticleUpdateResponse>(req);

            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                Console.WriteLine("Create Article Success");
                return res.Article;
            }
            Console.WriteLine("Call to Create Article failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            return null;
        }

        public async Task<bool> DeleteArticle(ArticleDeleteContract contract)
        {
            var req = new RestRequest("api/article/delete");
            var body = new ArticleDeleteRequest();
            body.Contract = contract;
            req.AddBody(body);

            var res = await client.PostAsync<ArticleDeleteResponse>(req);

            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                Console.WriteLine("Delete Article Success");
                return res.Success;
            }
            Console.WriteLine("Call to Delete Article failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            return false;
        }

        public async Task<ArticleGetResponseContract> GetArticle(string slug, int? userid)
        {
            var req = new RestRequest("api/article/getone");
            var body = new ArticleGetRequest();
            body.Article.Slug = slug;
            body.Article.FollowedById = userid;
            req.AddBody(body);

            var res = await client.PostAsync<ArticleGetResponse>(req);

            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                Console.WriteLine("GetArticle Success");
                return res.Article;
            }
            Console.WriteLine("Call to GetArticle failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            return null;
        }

        public async Task<List<ArticleGetResponseContract>> GetArticlesByFeed(ArticleGetContract request)
        {
            var req = new RestRequest("api/article/getmany");
            var body = new ArticleGetRequest();
            // Intentionally limited request object
            body.Article.FollowedById = request.FollowedById;
            body.Article.Offset = request.Offset;
            body.Article.Limit = request.Limit;
            req.AddBody(body);

            var res = await client.PostAsync<ArticleGetMultipleResponse>(req);

            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                Console.WriteLine("GetArticlesByFeed Success");
                return res.Articles;
            }
            Console.WriteLine("Call to GetArticlesByFeed failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            return null;
        }

        public async Task<List<ArticleGetResponseContract>> GetArticlesFiltered(ArticleGetContract request)
        {
            var req = new RestRequest("api/article/getmany");
            var body = new ArticleGetRequest();
            body.Article = request;
            req.AddBody(body);

            var res = await client.PostAsync<ArticleGetMultipleResponse>(req);

            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                Console.WriteLine("GetArticlesFiltered Success");
                return res.Articles;
            }
            Console.WriteLine("Call to GetArticlesFiltered failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            return null;
        }
    }
}

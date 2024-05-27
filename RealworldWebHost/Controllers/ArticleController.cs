using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Contracts.Communicator.Response;
using Contracts.Communicator.Request;
using Contracts.Validators;
using RealworldWebHost.DataAccess;
using RealworldWebHost.Models;
using RealworldWebHost.Utilities;

namespace RealworldWebHost.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController
    {
        private IArticleDA da;

        public ArticleController(IArticleDA da) { 
            this.da = da; }

        [HttpPost]
        [Route("/api/article/create")]
        public virtual IActionResult CreateArticle([FromBody] ArticleCreateRequest body)
        {
            Console.WriteLine("Call to WebHost.CreateArticle with title: " + body.Article.Title);
            ArticleCreateResponse resp = new ArticleCreateResponse();
            var validator = new CreateArticleValidator(body.Article);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.CreateArticle validation failure: " + validator.GetError().ToString()) ;
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }   
            string slug = da.CreateArticle(body.Article);
            if (string.IsNullOrEmpty(slug))
            {
                Console.WriteLine("WebHost.CreateArticle validation failure: " + validator.GetError().ToString());
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }
            resp.Error.ErrorCode = CALLER_ERR_CD.SUCCESS;
            resp.Slug = slug;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/article/update")]
        public virtual IActionResult UpdateArticle([FromBody] ArticleUpdateRequest body)
        {
            Console.WriteLine("Call to WebHost.UpdateArticle with slug: " + body.Article != null ? body.Article.Slug : "Missing!!");
            ArticleGetResponse resp = new ArticleGetResponse();
            var validator = new ArticleUpdateValidator(body.Article);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.UpdateArticle validation failure: " + validator.GetError().ToString()) ;
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            if (!da.UpdateArticle(body.Article))
            {
                Console.WriteLine("WebHost.UpdateArticle update failure");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }

            resp.Article = da.GetArticle(body.Article.Slug);
            if (resp.Article == null)
            {
                Console.WriteLine("WebHost.UpdateArticle retrieval failure");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }
            resp.Error.ErrorCode = CALLER_ERR_CD.SUCCESS;
            return new ObjectResult(null);
        }

        [HttpPost]
        [Route("/api/article/getone")]
        public virtual IActionResult GetArticle([FromBody] ArticleGetRequest body)
        {
            Console.WriteLine("Call to WebHost.GetArticle with slug: " +  body.Article != null ? body.Article.Slug : "Missing!!");
            ArticleGetResponse resp = new ArticleGetResponse();
            var validator = new ArticleGetOneValidator(body.Article);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.GetArticle validation failure: " + validator.GetError().ToString());
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }

            resp.Article = da.GetArticle(body.Article.Slug);
            if (resp.Article == null)
            {
                Console.WriteLine("WebHost.GetArticle retrieval failure");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }

            resp.Error.ErrorCode = CALLER_ERR_CD.SUCCESS;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/article/getmany")]
        public virtual IActionResult GetArticles([FromBody] ArticleGetRequest body)
        {
            Console.WriteLine("Call to WebHost.GetArticles");
            ArticleGetMultipleResponse resp = new ArticleGetMultipleResponse();
            var validator = new ArticleGetMultipleValidator(body.Article);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.GetArticles validation failure: " + validator.GetError().ToString());
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }

            resp.Articles = da.GetArticlesFiltered(body.Article);
            if (resp.Articles == null)
            {
                Console.WriteLine("WebHost.GetArticles retrieval failure");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }
            resp.Error.ErrorCode = CALLER_ERR_CD.SUCCESS;
            return new ObjectResult(resp);
        }
    }
}

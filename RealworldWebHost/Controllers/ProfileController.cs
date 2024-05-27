using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using Contracts.Validators;
using Microsoft.AspNetCore.Mvc;
using RealworldWebHost.DataAccess;

namespace RealworldWebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController
    {
        private IProfileDA da;
        private IArticleDA articleDA;
        public ProfileController(IProfileDA da, IArticleDA articleDA)
        {
            this.da = da;
            this.articleDA = articleDA;
        }

        [HttpPost]
        [Route("/api/profile/get")]
        public virtual IActionResult GetProfile([FromBody] ProfileGetRequest body)
        {
            Console.WriteLine("Call to WebHost.GetProfile with FollowedUsername: " + body.Profile.FollowedUsername);
            ProfileGetResponse resp = new ProfileGetResponse();
            var validator = new ProfileGetValidator(body.Profile);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.GetProfile validation failure: " + validator.GetError().ToString());
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            var profile = da.GetProfile(body.Profile.FollowedUsername, body.Profile.UserId);
            if (profile == null)
            {
                Console.WriteLine("Call to WebHost.GetProfile failed at DA");
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = "Internal Server Error";
                return new ObjectResult(resp);
            }
            resp.Profile = profile;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/profile/follow")]
        public virtual IActionResult Follow([FromBody] ProfileFollowRequest body)
        {
            Console.WriteLine("Call to WebHost.Follow with FollowedUsername: " + body.Contract.FollowedUsername);
            ProfileGetResponse resp = new ProfileGetResponse();
            var validator = new ProfileFollowValidator(body.Contract);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.Follow validation failure: " + validator.GetError().ToString());
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            bool success = da.Follow(body.Contract.UserId, body.Contract.FollowedUsername);
            if (!success)
            {
                Console.WriteLine("Call to WebHost.Follow failed at DA");
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = "Internal Server Error";
                return new ObjectResult(resp);
            }

            var r = da.GetProfile(body.Contract.FollowedUsername, body.Contract.UserId);
            if (r == null)
            {
                Console.WriteLine("Call to WebHost.Follow failed at DA.GetProfile");
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = "Internal Server Error";
                return new ObjectResult(resp);
            }

            resp.Error.ID = CALLER_ERR_CD.SUCCESS;
            resp.Profile = r;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/profile/unfollow")]
        public virtual IActionResult Unfollow([FromBody] ProfileFollowRequest body)
        {
            Console.WriteLine("Call to WebHost.Unfollow with FollowedUsername: " + body.Contract.FollowedUsername);
            ProfileGetResponse resp = new ProfileGetResponse();
            var validator = new ProfileFollowValidator(body.Contract);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.Unfollow validation failure: " + validator.GetError().ToString());
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            bool success = da.StopFollowing(body.Contract.UserId, body.Contract.FollowedUsername);
            if (!success)
            {
                Console.WriteLine("Call to WebHost.Unfollow failed at DA");
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = "Internal Server Error";
                return new ObjectResult(resp);
            }

            var r = da.GetProfile(body.Contract.FollowedUsername, body.Contract.UserId);
            if (r == null)
            {
                Console.WriteLine("Call to WebHost.Unfollow failed at DA.GetProfile");
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = "Internal Server Error";
                return new ObjectResult(resp);
            }

            resp.Error.ID = CALLER_ERR_CD.SUCCESS;
            resp.Profile = r;
            return new ObjectResult(resp);
        }


        [HttpPost]
        [Route("/api/profile/favorite")]
        public virtual IActionResult Favorite([FromBody] ProfileFavoriteRequest body)
        {
            Console.WriteLine("Call to WebHost.Favorite with Slug: " + body.Contract.ArticleSlug);
            ArticleGetResponse resp = new ArticleGetResponse();
            var validator = new ProfileFavoriteValidator(body.Contract);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.Favorite validation failure: " + validator.GetError().ToString());
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            bool success = da.SetFavorite(body.Contract.UserId, body.Contract.ArticleSlug);
            if (!success)
            {
                Console.WriteLine("Call to WebHost.Favorite failed at DA");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }

            var r = articleDA.GetArticle(body.Contract.ArticleSlug, body.Contract.UserId);
            if (r == null)
            {
                Console.WriteLine("Call to WebHost.Favorite failed at ArticleDA.GetArticle");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }

            resp.Error.ErrorCode = CALLER_ERR_CD.SUCCESS;
            resp.Article = r;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/profile/unfavorite")]
        public virtual IActionResult Unfavorite([FromBody] ProfileFavoriteRequest body)
        {
            Console.WriteLine("Call to WebHost.Unfavorite with Slug: " + body.Contract.ArticleSlug);
            ArticleGetResponse resp = new ArticleGetResponse();
            var validator = new ProfileFavoriteValidator(body.Contract);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.Unfavorite validation failure: " + validator.GetError().ToString());
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            bool success = da.DeleteFavorite(body.Contract.UserId, body.Contract.ArticleSlug);
            if (!success)
            {
                Console.WriteLine("Call to WebHost.Unfavorite failed at DA");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }

            var r = articleDA.GetArticle(body.Contract.ArticleSlug, body.Contract.UserId);
            if (r == null)
            {
                Console.WriteLine("Call to WebHost.Unfavorite failed at ArticleDA.GetArticle");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }

            resp.Error.ErrorCode = CALLER_ERR_CD.SUCCESS;
            resp.Article = r;
            return new ObjectResult(resp);
        }
    }
}

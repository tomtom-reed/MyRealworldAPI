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
    public class CommentController
    {
        private ICommentDA da;
        public CommentController(ICommentDA da) { this.da = da; }

        [HttpPost]
        [Route("/api/comment/create")]
        public virtual IActionResult CreateComment([FromBody] CommentCreateRequest body)
        {
            Console.WriteLine("Call to WebHost.CreateComment with slug: " + body.Comment.ArticleSlug); // TODO null pointer handling
            CommentCreateResponse resp = new CommentCreateResponse(); // could use Get but eyyyy
            var validator = new CommentCreateValidator(body.Comment);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.CreateComment validation failure: " + validator.GetError().ToString());
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            int commentId = da.CreateComment(body.Comment.ArticleSlug, body.Comment.AuthorId, body.Comment.Body);
            if (commentId < 0)
            {
                Console.WriteLine("Call to WebHost.CreateComment failed at DA");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                return new ObjectResult(resp);
            }
            resp.CommentId = commentId;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/comment/delete")]
        public virtual IActionResult DeleteComment([FromBody] CommentDeleteRequest body)
        {
            Console.WriteLine("Call to WebHost.DeleteComment with slug: " + body.Comment.Slug);
            CommentDeleteResponse resp = new CommentDeleteResponse();
            var validator = new CommentDeleteValidator(body.Comment);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.DeleteComment validation failure: " + validator.GetError().ToString());
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            bool success = da.DeleteComment(body.Comment.Slug, body.Comment.CommentId, body.Comment.AuthorId);
            if (!success)
            {
                Console.WriteLine("Call to WebHost.DeleteComment failed at DA");
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Internal Server Error";
                resp.Success = false;
                return new ObjectResult(resp);
            }
            resp.Success = true;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/comment/getmany")]
        public virtual IActionResult GetComments([FromBody] CommentGetRequest body)
        {
            Console.WriteLine("Call to WebHost.GetComment with commentId: " + body.Comment.CommentId);
            CommentsGetResponse resp = new CommentsGetResponse();
            var validator = new CommentGetOneValidator(body.Comment);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.GetComment validation failure: " + validator.GetError().ToString());
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            List<CommentGetResponseContract> comments = da.GetByArticle(body.Comment.Slug, body.Comment.FollowerId);
            if (comments == null) // length can be zero
            {
                Console.WriteLine("Call to WebHost.GetComment failed at DA");
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = "Internal Server Error";
                return new ObjectResult(resp);
            }
            resp.Comments = comments;
            return new ObjectResult(resp);
        }

        [HttpPost]
        [Route("/api/comment/getone")]
        public virtual IActionResult GetComment([FromBody] CommentGetRequest body)
        {
            Console.WriteLine("Call to WebHost.GetComment with commentId: " + body.Comment.CommentId);
            CommentGetResponse resp = new CommentGetResponse();
            var validator = new CommentGetOneValidator(body.Comment);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.GetComment validation failure: " + validator.GetError().ToString());
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = validator.GetError().Message;
                return new ObjectResult(resp);
            }
            var c = da.GetById(body.Comment.Slug, (int)body.Comment.CommentId, body.Comment.FollowerId);
            if (c == null || c.Id < 0)
            {
                Console.WriteLine("Call to WebHost.GetComment failed at DA");
                resp.Error.ID = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.Message = "Internal Server Error";
                return new ObjectResult(resp);
            }
            resp.Comment = c;
            return new ObjectResult(resp);
        }
    }
}

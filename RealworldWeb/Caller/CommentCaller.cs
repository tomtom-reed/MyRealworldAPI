﻿using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using RealworldWeb.Utils;
using RestSharp;

namespace RealworldWeb.Caller
{

    public interface ICommentCaller
    {
        public Task<CommentGetResponseContract> CreateComment(CommentCreateContract request);
        public Task<string> DeleteComment(CommentDeleteContract request);
        public Task<List<CommentGetResponseContract>> GetAllComments(string slug, int? userid);
        public Task<CommentGetResponseContract> GetComment(string slug, int commentId, int? userid);
    }
    public class CommentCaller : ICommentCaller
    {
        RestClient client;
        public CommentCaller(WebConfiguration config)
        {
            var options = new RestClientOptions(config.Connections_WebHost);
            this.client = new RestClient(options);
        }

        public async Task<CommentGetResponseContract> CreateComment(CommentCreateContract request)
        {
            var req = new RestRequest("/api/comment/create");
            var body = new CommentCreateRequest();
            body.Comment = request;
            req.AddBody(body);

            try
            {
                var res = await client.PostAsync<CommentCreateResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Comment Created with ID: " + res.CommentId);
                    return await this.GetComment(request.ArticleSlug, res.CommentId, request.AuthorId);
                }
                Console.WriteLine("Call to CreateComment failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateComment failed: " + e.Message);
            }
            return null;
        }

        public async Task<string> DeleteComment(CommentDeleteContract request)
        {
            var req = new RestRequest("/api/comment/delete");
            var body = new CommentDeleteRequest();
            body.Comment = request;
            req.AddBody(body);

            try
            {
                var res = await client.PostAsync<CommentDeleteResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Comment Deleted");
                    return "Success";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DeleteComment failed: " + e.Message);
            }
            return null;
        }

        public async Task<List<CommentGetResponseContract>> GetAllComments(string slug, int? userid)
        {
            var req = new RestRequest("/api/comment/getmany");
            var body = new CommentGetRequest();
            body.Comment.Slug = slug;
            body.Comment.FollowerId = userid;
            req.AddBody(body);

            try
            {
                var res = await client.PostAsync<CommentsGetResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Comments retrieved");
                    return res.Comments;
                }
                Console.WriteLine("Call to CreateComment failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllComments failed: " + e.Message);
            }
            return null;
        }

        public async Task<CommentGetResponseContract> GetComment(string slug, int commentId, int? userid)
        {
            var req = new RestRequest("/api/comment/getone");
            var body = new CommentGetRequest();
            body.Comment.Slug = slug;
            body.Comment.CommentId = commentId;
            body.Comment.FollowerId = userid;
            req.AddBody(body);

            try
            {
                var res = await client.PostAsync<CommentGetResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Comment retrieved");
                    return res.Comment;
                }
                Console.WriteLine("Call to CreateComment failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("GetComment failed: " + e.Message);
            }
            return null;
        }
    }
}

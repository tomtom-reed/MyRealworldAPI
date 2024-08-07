﻿using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using RealworldWeb.Utils;
using RestSharp;

namespace RealworldWeb.Caller
{
    public interface IFavoriteCaller
    {
        Task<ArticleGetResponseContract> FavoriteArticle(ProfileFavoriteContract request);
        Task<ArticleGetResponseContract> UnfavoriteArticle(ProfileFavoriteContract request);
    }
    public class FavoriteCaller : IFavoriteCaller
    {
        RestClient client;

        public FavoriteCaller(WebConfiguration config)
        {
            var options = new RestClientOptions(config.Connections_WebHost);
            this.client = new RestClient(options);
        }

        public async Task<ArticleGetResponseContract> FavoriteArticle(ProfileFavoriteContract request)
        {
            var req = new RestRequest("api/profile/favorite");
            var body = new ProfileFavoriteRequest();
            body.Contract = request;
            req.AddBody(body);

            try
            {
                var res = await client.PostAsync<ArticleGetResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Favorite Success");
                    return res.Article;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Favorite failed: " + e.Message);
            }
            Console.WriteLine("Favorite failed");
            return null;
        }

        public async Task<ArticleGetResponseContract> UnfavoriteArticle(ProfileFavoriteContract request)
        {
            var req = new RestRequest("api/profile/unfavorite");
            var body = new ProfileFavoriteRequest();
            body.Contract = request;
            req.AddBody(body);

            try
            {
                var res = await client.PostAsync<ArticleGetResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Unfavorite Success");
                    return res.Article;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unfavorite failed: " + e.Message);
            }
            Console.WriteLine("Unfavorite failed");
            return null;
        }

    }
}

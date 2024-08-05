using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using RealworldWeb.Utils;
using RestSharp;

namespace RealworldWeb.Caller
{
    public interface IProfileCaller
    {
        Task<ProfileGetResponseContract> GetProfile(ProfileGetContract request);
        Task<ProfileGetResponseContract> Follow(ProfileFollowContract request);
        Task<ProfileGetResponseContract> Unfollow(ProfileFollowContract request);
    }
    public class ProfileCaller : IProfileCaller
    {
        RestClient client;
        public ProfileCaller(WebConfiguration config)
        {
            var options = new RestClientOptions(config.Connections_WebHost);
            this.client = new RestClient(options);
        }

        public async Task<ProfileGetResponseContract> GetProfile(ProfileGetContract request)
        {
            var req = new RestRequest("api/profile/get");
            var body = new ProfileGetRequest();
            body.Profile = request;
            req.AddBody(request);

            try
            {
                var res = await client.PostAsync<ProfileGetResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("GetProfile Success");
                    return res.Profile;
                }
                Console.WriteLine("Call to GetProfile failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("GetProfile failed: " + e.Message);
            }
            return null;
        }

        public async Task<ProfileGetResponseContract> Follow(ProfileFollowContract request)
        {
            var req = new RestRequest("api/profile/follow");
            var body = new ProfileFollowRequest();
            body.Contract = request;
            req.AddBody(request);

            try
            {
                var res = await client.PostAsync<ProfileGetResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Follow Success");
                    return res.Profile;
                }
                Console.WriteLine("Call to Follow failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("Follow failed: " + e.Message);
            }
            return null;
        }

        public async Task<ProfileGetResponseContract> Unfollow(ProfileFollowContract request)
        {
            var req = new RestRequest("api/profile/unfollow");
            var body = new ProfileFollowRequest();
            body.Contract = request;
            req.AddBody(request);

            try
            {
                var res = await client.PostAsync<ProfileGetResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Follow Success");
                    return res.Profile;
                }
                Console.WriteLine("Call to Follow failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("Follow failed: " + e.Message);
            }
            return null;
        }
    }
}

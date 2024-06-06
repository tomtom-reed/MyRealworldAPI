using Contracts.Communicator.Response;
using RestSharp;
using RestSharp.Authenticators;

using Contracts.Communicator.Request;
using Contracts.Models;

namespace RealworldWeb.Caller
{
    public interface IUserCaller
    {
        Task<UserDetailsResponseElement?> Login(LoginContract login);
        Task<UserDetailsResponseElement> GetUserDetailsByEmailAsync(string email);
        Task<UserDetailsResponseElement> GetUserDetailsByUsernameAsync(string username);
        Task<UserDetailsResponseElement> GetUserDetailsByIdAsync(int userId);
        Task<bool> CreateUserAsync(UserCreateContract user);
        Task<UserDetailsResponseElement> UpdateUser(UserUpdateContract user);
    }
    public class UserCaller : IUserCaller
    {

        RestClient client;
        
        public UserCaller(string webhostUrl) {
            var options = new RestClientOptions(webhostUrl); 
            this.client = new RestClient(options);
        }


        public async Task<UserDetailsResponseElement?> Login(LoginContract login)
        {
            var req = new RestRequest("api/user/login");
            var body = new LoginRequest();
            body.Login = login;
            req.AddBody(body);

            var res = await client.PostAsync<UserDetailsResponse>(req);

            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                Console.WriteLine("Login Success");
                return res.User;
            } 
            Console.WriteLine("Call to Login failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            return null;
        }


        public async Task<UserDetailsResponseElement> GetUserDetailsByEmailAsync(string email)
        {
            var req = new RestRequest("/api/user/getDetails");
            UserDetailsRequest body = new UserDetailsRequest();
            body.UserDetails.Email = email;
            req.AddJsonBody<UserDetailsRequest>(body);

            var res = await client.PostAsync<UserDetailsResponse>(req);
            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                return res.User;
            }
            //
            return new UserDetailsResponseElement(); // null
        }

        public async Task<UserDetailsResponseElement> GetUserDetailsByUsernameAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDetailsResponseElement> GetUserDetailsByIdAsync(int userid)
        {
            throw new NotImplementedException();
        }


        // The Create User API needs UserDetails, but that is handled in a separate function. Two backend calls.
        // So this just returns true or false, whether the user was created successfully. 
        public async Task<bool> CreateUserAsync(UserCreateContract user)
        {
            var req = new RestRequest("/api/user/create");
            UserCreateRequest body = new UserCreateRequest();
            body.User = user;
            req.AddJsonBody<UserCreateRequest>(body);

            var res = await client.PostAsync<UserCreateResponse> (req);
            if (res != null && res.Error != null && res.Error.ErrorCode != CALLER_ERR_CD.SUCCESS)
            {
                return true;
            }
            return false;
        }

        public async Task<UserDetailsResponseElement> UpdateUser(UserUpdateContract user)
        {
            UserUpdateRequest body = new UserUpdateRequest();
            body.User = user;

            var req = new RestRequest("/api/user/update");
            req.AddBody(body);
            var res = await client.PostAsync<UserDetailsResponse>(req);
            if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
            {
                return res.User;
            }
            //
            return new UserDetailsResponseElement(); // null
        }
    }
}

using Contracts.Communicator.Response;
using RestSharp;
using RestSharp.Authenticators;

using Contracts.Communicator.Request;
using Contracts.Models;
using RealworldWeb.Utils;

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
        
        public UserCaller(WebConfiguration config) {
            var options = new RestClientOptions(config.Connections_WebHost); 
            this.client = new RestClient(options);
        }


        public async Task<UserDetailsResponseElement?> Login(LoginContract login)
        {
            var req = new RestRequest("api/user/login");
            var body = new LoginRequest();
            body.Login = login;
            req.AddBody(body);

            try
            {
                var res = await client.PostAsync<UserDetailsResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    Console.WriteLine("Login Success");
                    return res.User;
                }
                Console.WriteLine("Call to Login failed" + ((res != null && res.Error != null) ? " with error: " + res.Error.ErrorMessage : ""));
            }
            catch (Exception e)
            {
                Console.WriteLine("Login failed: " + e.Message);
            }
            return null;
        }

        public async Task<UserDetailsResponseElement> GetUserDetailsByEmailAsync(string email)
        {
            var req = new RestRequest("/api/user/getDetailsByEmail");
            UserDetailsByEmailRequest body = new UserDetailsByEmailRequest();
            body.Email = email;
            req.AddJsonBody<UserDetailsByEmailRequest>(body);
            try
            {
                var res = await client.PostAsync<UserDetailsResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    return res.User;
                }
            } catch (Exception e)
            {
                Console.WriteLine("GetUserDetailsByEmailAsync failed: " + e.Message);
            }
            //
            return new UserDetailsResponseElement(); // null
        }

        public async Task<UserDetailsResponseElement> GetUserDetailsByUsernameAsync(string username)
        {
            var req = new RestRequest("/api/user/getDetailsByUsername");
            UserDetailsByUsernameRequest body = new UserDetailsByUsernameRequest();
            body.Username = username;
            req.AddJsonBody<UserDetailsByUsernameRequest>(body);
            try
            {
                var res = await client.PostAsync<UserDetailsResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    return res.User;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUserDetailsByUsernameAsync failed: " + e.Message);
            }
            //
            return new UserDetailsResponseElement(); // null
        }

        public async Task<UserDetailsResponseElement> GetUserDetailsByIdAsync(int userid)
        {
            var req = new RestRequest("/api/user/getDetailsById");
            UserDetailsByIdRequest body = new UserDetailsByIdRequest();
            body.Id = userid;
            req.AddJsonBody<UserDetailsByIdRequest>(body);
            try
            {
                var res = await client.PostAsync<UserDetailsResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    return res.User;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUserDetailsByIdAsync failed: " + e.Message);
            }
            //
            return new UserDetailsResponseElement(); // null
        }


        // The Create User API needs UserDetails, but that is handled in a separate function. Two backend calls.
        // So this just returns true or false, whether the user was created successfully. 
        public async Task<bool> CreateUserAsync(UserCreateContract user)
        {
            var req = new RestRequest("/api/user/create");
            UserCreateRequest body = new UserCreateRequest();
            body.User = user;
            req.AddJsonBody<UserCreateRequest>(body);

            try
            {
                var res = await client.PostAsync<UserCreateResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateUserAsync failed: " + e.Message);
            }
            return false;
        }

        public async Task<UserDetailsResponseElement> UpdateUser(UserUpdateContract user)
        {
            UserUpdateRequest body = new UserUpdateRequest();
            body.User = user;

            var req = new RestRequest("/api/user/update");
            req.AddBody(body);
            try
            {
                var res = await client.PostAsync<UserDetailsResponse>(req);
                if (res != null && (res.Error == null || res.Error.ErrorCode == CALLER_ERR_CD.SUCCESS))
                {
                    return res.User;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateUser failed: " + e.Message);
            }
            //
            return new UserDetailsResponseElement(); // null
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Contracts.Communicator.Response;
using Contracts.Communicator.Request;
using Contracts.Validators;
using NSec.Cryptography;
using RealworldWebHost.DataAccess;
using RealworldWebHost.Models;
using RealworldWebHost.Utilities;

namespace RealworldWebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserDA da;
        private ISecUtils secUtils;

        public UserController(IUserDA da, ISecUtils secUtils)
        {
            this.da = da;
            this.secUtils = secUtils;
        }
        /// <summary>
        /// Create a user and return the info necessary to create a token
        /// </summary>
        [HttpPost]
        [Route("/api/user/create")]
        public virtual IActionResult CreateUser([FromBody] UserCreateRequest body)
        {
            Console.WriteLine("Call to WebHost.CreateUser with username: " + body.User.Username);
            var validator = new CreateUserValidator(body.User);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.CreateUser validation failure: " + validator.GetError().ToString()) ;
                // Error Handling, premature return
            }
            bool success = da.CreateUser(body.User.Username, body.User.Email, body.User.Password);

            if (success)
            {
                return new ObjectResult(GetUserDetails(body.User.Email));
            }
            else
            {
                // error handling 
            }

            string exampleJson = null;
            exampleJson = "{\n  \"comment\" : {\n    \"createdAt\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"author\" : {\n      \"image\" : \"image\",\n      \"following\" : true,\n      \"bio\" : \"bio\",\n      \"username\" : \"username\"\n    },\n    \"id\" : 0,\n    \"body\" : \"body\",\n    \"updatedAt\" : \"2000-01-23T04:56:07.000+00:00\"\n  }\n}";
            var example = exampleJson != null
                ? JsonSerializer.Deserialize<UserCreateResponse>(exampleJson)
                : default(UserCreateResponse);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Check if a user login request succeeds
        /// </summary>
        [HttpPost]
        [Route("/api/user/login")]
        [ProducesResponseType<UserDetailsResponse>(200)]
        public virtual IActionResult LoginUser([FromBody] LoginRequest body)
        {
            UserDetailsResponse response = new UserDetailsResponse();
            var validator = new UserLoginValidator(body.Login);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.LoginUser validation failure: " + validator.GetError().ToString());
                response.Error = new ErrorResponse();
                response.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                response.Error.ErrorMessage = "WebHost.LoginUser validation failure: " + validator.GetError().ToString();
                //yield return response;
                return StatusCode(400, response);
            }
            
            UserDetails deets = da.GetUserDetails(body.Login.Email);
            if (!String.IsNullOrEmpty(deets.ErrorMsg))
            {
                string msg = "WebHost.LoginUser DA failure: " + deets.ErrorMsg;
                Console.WriteLine(msg);
                response.Error = new ErrorResponse();
                response.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                response.Error.ErrorMessage = msg;
                return StatusCode(401, response);
            }

            if (!secUtils.ComparePasswordToHash(body.Login.Password, deets.PasswordHash))
            {
                //Console.WriteLine("WebHost.LoginUser wrong password");
                response.Error = new ErrorResponse();
                response.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                response.Error.ErrorMessage = "WebHost.LoginUser wrong password";
                return StatusCode(401, response);
            }
            response = this.UserDetailsToResponse(deets);
            return StatusCode(200, response);
        }

        [HttpPost]
        [Route("/api/user/getDetails")]
        public virtual IActionResult GetUserDetails([FromBody] UserDetailsRequest body)
        {
            // Validation and whatever? 
            var respbody = GetUserDetails(body.UserDetails.Email); 
            return new ObjectResult(respbody);
        }

        private UserDetailsResponse GetUserDetails(string email)
        {
            UserDetails deets = da.GetUserDetails(email);
            if (!String.IsNullOrEmpty(deets.ErrorMsg))
            {
                Console.Write("GetUserDetails Error: " + deets.ErrorMsg);
                // TODO Error Handling
            }
            return UserDetailsToResponse(deets);
        }

        private UserDetailsResponse UserDetailsToResponse(UserDetails deets)
        {
            UserDetailsResponse resp = new UserDetailsResponse();
            resp.User = new UserDetailsResponseElement();
            resp.User.Email = deets.Email;
            resp.User.Username = deets.UserName;
            resp.User.Bio = deets.Bio == null ? "" : deets.Bio;
            resp.User.Image = deets.Img == null ? "" : deets.Img;
            // For successes only, no need to initialize Error
            return resp;
        }

        [HttpPost]
        [Route("/api/user/update")]
        public virtual IActionResult UpdateUser([FromBody] UserUpdateRequest body)
        {
            UserDetailsResponse resp = new UserDetailsResponse();
            if (body == null || body.User == null)
            {
                resp.Error = new ErrorResponse();
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Empty Request";
                return StatusCode(200, resp);
            }
            var validator = new UserUpdateValidator(body.User);
            if (!validator.Validate())
            {
                // Error handling 
                resp.Error = new ErrorResponse();
                resp.Error.ErrorCode = CALLER_ERR_CD.GENERIC_ERROR;
                resp.Error.ErrorMessage = "Bad Request";
                return StatusCode(200, resp);
            }
            
            bool updateSuccess = da.UpdateUserDetails(body.User);
            if (updateSuccess)
            {
                resp = GetUserDetails(body.User.Email != null ? body.User.Email : body.User.UserCurrentEmail);
            }
            
            return new ObjectResult(resp);
        }
    }
}

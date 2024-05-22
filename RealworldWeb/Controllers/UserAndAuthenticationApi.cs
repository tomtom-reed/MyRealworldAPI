/*
 * RealWorld Conduit API
 *
 * Conduit API documentation
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using RealworldApi.Web.Attributes;
using RealworldApi.Web.Security;
using Microsoft.AspNetCore.Authorization;
using RealworldApi.Web.Models;
using Contracts.Models;
using Contracts.Validators;
using RealworldWeb.Caller;
using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using System.Security.Claims;

namespace RealworldApi.Web.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class UserAndAuthenticationApiController : ControllerBase
    {
        private readonly ITokenUtils tokenizer;
        private readonly IUserCaller caller;

        public UserAndAuthenticationApiController(ITokenUtils tokenizer, IUserCaller caller)
        {
            this.tokenizer = tokenizer;
            this.caller = caller;
        }


        private async Task<IActionResult> GetUserDetails(string email) 
        {
            UserDetailsResponseElement deets = await caller.GetUserDetailsAsync(email);
            if (deets == null)
            {
                return StatusCode(422, default(GenericErrorModel)); // TODO
            }
            InlineResponse200 respBody = new InlineResponse200(); // lies. LIES! It's 201, not 200! For create. Specifically. 
            respBody.User.Username = deets.Username;
            respBody.User.Email = deets.Email;
            respBody.User.Bio = deets.Bio;
            respBody.User.Image = deets.Image;
            respBody.User.Token = tokenizer.GetToken(deets.Email);
            return new ObjectResult(respBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Register a new user</remarks>
        /// <param name="body">Details of the new user to register</param>
        /// <response code="201">User</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/users")]
        [ValidateModelState]
        [SwaggerOperation("CreateUser")]
        [SwaggerResponse(statusCode: 201, type: typeof(InlineResponse200), description: "User")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody]NewUser body)
        {
            //UserContract usr = new UserContract();
            UserCreateContract usr = new UserCreateContract();
            usr.Email = body.Email;
            usr.Username = body.Username;
            usr.Password = body.Password;

            var validator = new CreateUserValidator(usr);
            if (!validator.Validate())
            {
                Console.WriteLine("CreateUser validation failure: " + validator.GetError().Message);
                return StatusCode(422, default(GenericErrorModel));
            }
            // call WebHost
            var createSuccess = await caller.CreateUserAsync(usr);
            if (createSuccess) {
                return StatusCode(422, default(GenericErrorModel));
            }

            UserDetailsResponseElement deetsResp = await caller.GetUserDetailsAsync(usr.Email);
            if (deetsResp == null)
            {
                return StatusCode(422, default(GenericErrorModel));
            }

            InlineResponse200 respBody = new InlineResponse200(); // lies. LIES! It's 201, not 200! 
            respBody.User.Username = deetsResp.Username;
            respBody.User.Email = deetsResp.Email;
            respBody.User.Bio = deetsResp.Bio;
            respBody.User.Image = deetsResp.Image;
            respBody.User.Token = tokenizer.GetToken(deetsResp.Email);

            return StatusCode(201, respBody);
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <remarks>Gets the currently logged-in user</remarks>
        /// <response code="200">User</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpGet]
        [Route("/api/user")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("GetCurrentUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse200), description: "User")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (User.Claims == null || User.Claims.Count() == 0)
            {
                Console.WriteLine("Authentication must have failed");
                return StatusCode(401);
            }
            var claimslist = User.Claims.ToList();
            Claim nameclaim = claimslist.FirstOrDefault(c => c.Type == "unique_name"); // ClaimTypes.Name);
            if (nameclaim == null)
            {
                Console.WriteLine("NameClaim is null");
                return StatusCode(422, default(GenericErrorModel));
            }

            var deets = await GetUserDetails(nameclaim.Value);
            return deets;
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse200));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
        }

        /// <summary>
        /// Existing user login
        /// </summary>
        /// <remarks>Login for existing user</remarks>
        /// <param name="body">Credentials to use</param>
        /// <response code="200">User</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/users/login")]
        [ValidateModelState]
        [SwaggerOperation("Login")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse200), description: "User")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginUser body)
        {
            LoginContract req = new LoginContract();
            req.Email = body.Email;
            req.Password = body.Password;
            UserLoginValidator validator = new UserLoginValidator(req);
            if (!validator.Validate())
            {
                Console.WriteLine(validator.GetError().ToString());
                return StatusCode(422, default(GenericErrorModel));
            }

            UserDetailsResponseElement? resp = await caller.Login(req);
            if (resp == null)
            {
                Console.WriteLine("Call to UserCaller.Login returned null response");
                return StatusCode(401);
            }

            InlineResponse200 respBody = new InlineResponse200();
            respBody.User.Username = resp.Username;
            respBody.User.Email = resp.Email;
            respBody.User.Bio = resp.Bio;
            respBody.User.Image = resp.Image;
            respBody.User.Token = tokenizer.GetToken(resp.Email);

            return StatusCode(200, respBody);
        }

        /// <summary>
        /// Update current user
        /// </summary>
        /// <remarks>Updated user information for current user</remarks>
        /// <param name="body">User details to update. At least **one** field is required.</param>
        /// <response code="200">User</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPut]
        [Route("/api/user")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("UpdateCurrentUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse200), description: "User")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> UpdateCurrentUser([FromBody]UpdateUser body)
        {
            // Step 1: Get the token 
            if (User.Claims == null || User.Claims.Count() == 0)
            {
                Console.WriteLine("Authentication must have failed");
                return StatusCode(401);
            }
            var claimslist = User.Claims.ToList();
            Claim? nameclaim = claimslist.FirstOrDefault(c => c.Type == "unique_name"); // ClaimTypes.Name);
            if (nameclaim == null)
            {
                Console.WriteLine("NameClaim is null");
                return StatusCode(422, default(GenericErrorModel));
            }

            // Step 2:  Validator.
            //      Fail if everything is null
            //      null || policy
            UserUpdateContract req = new UserUpdateContract();
            req.UserCurrentEmail = nameclaim.Value;
            req.Email = body.Email;
            req.Username = body.Username;
            req.Password = body.Password;
            req.Bio = body.Bio;
            req.Image = body.Image;
            UserUpdateValidator validator = new UserUpdateValidator(req);
            if (!validator.Validate())
            {
                Console.WriteLine(validator.GetError()); 
                return StatusCode(422, default(GenericErrorModel));
            }

            // Step 3: Caller, etc 
            // proc dynamically builds query 
            // it IS possible to update email, so the proc first needs to make sure there's no collision. Transaction!

            var resp = await caller.UpdateUser(req);
            InlineResponse200 respBody = new InlineResponse200();
            respBody.User.Username = resp.Username;
            respBody.User.Email = resp.Email;
            respBody.User.Bio = resp.Bio;
            respBody.User.Image = resp.Image;
            respBody.User.Token = tokenizer.GetToken(resp.Email);

            return StatusCode(200, respBody);


            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse200));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
        }
    }
}

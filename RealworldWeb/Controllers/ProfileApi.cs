/*
 * RealWorld Conduit API
 *
 * Conduit API documentation
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
//using System.Text.Json;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using RealworldApi.Web.Attributes;
using RealworldApi.Web.Security;
using Microsoft.AspNetCore.Authorization;
using RealworldApi.Web.Models;
using RealworldWeb.Caller;
using Contracts.Communicator.Request;
using Contracts.Communicator.Response;
using Contracts.Validators;

namespace RealworldApi.Web.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ProfileApiController : ControllerBase
    {
        private ITokenUtils tokenizer;
        private IProfileCaller caller;
        public ProfileApiController(ITokenUtils tokenizer, IProfileCaller caller)
        {
            this.tokenizer = tokenizer;
            this.caller = caller;
        }
        private Profile ConvertProfile(ProfileGetResponseContract contract) { 
            Profile profile = new Profile();
            profile.Bio = contract.Bio;
            profile.Following = contract.Following;
            profile.Image = contract.Image;
            profile.Username = contract.Username;
            return profile;
        }
        /// <summary>
        /// Follow a user
        /// </summary>
        /// <remarks>Follow a user by username</remarks>
        /// <param name="username">Username of the profile you want to follow</param>
        /// <response code="200">Profile</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/profiles/{username}/follow")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("FollowUserByUsername")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2001), description: "Profile")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> FollowUserByUsername([FromRoute][Required]string username)
        {
            int? userid = tokenizer.GetIdFromAuthedUser(User);
            if (userid == null)
            {
                Console.WriteLine("Authentication must have failed");
                return StatusCode(401);
            }
            var contract = new ProfileFollowContract();
            contract.FollowedUsername = username;
            contract.UserId = userid.Value;
            var validator = new ProfileFollowValidator(contract);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.Follow validation failure: " + validator.GetError().ToString());
                return StatusCode(422, default(GenericErrorModel));
            }
            var profile = await caller.Follow(contract);
            if (profile == null)
            {
                return StatusCode(422, default(GenericErrorModel));
            }
            InlineResponse2001 resp = new InlineResponse2001();
            resp.Profile = ConvertProfile(profile);
            return StatusCode(200, resp);
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2001));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
        }

        /// <summary>
        /// Unfollow a user
        /// </summary>
        /// <remarks>Unfollow a user by username</remarks>
        /// <param name="username">Username of the profile you want to unfollow</param>
        /// <response code="200">Profile</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpDelete]
        [Route("/api/profiles/{username}/follow")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("UnfollowUserByUsername")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2001), description: "Profile")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> UnfollowUserByUsername([FromRoute][Required] string username)
        {
            int? userid = tokenizer.GetIdFromAuthedUser(User);
            if (userid == null)
            {
                Console.WriteLine("Authentication must have failed");
                return StatusCode(401);
            }
            var contract = new ProfileFollowContract();
            contract.FollowedUsername = username;
            contract.UserId = userid.Value;
            var validator = new ProfileFollowValidator(contract);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.Follow validation failure: " + validator.GetError().ToString());
                return StatusCode(422, default(GenericErrorModel));
            }
            var profile = await caller.Unfollow(contract);
            if (profile == null)
            {
                return StatusCode(422, default(GenericErrorModel));
            }
            InlineResponse2001 resp = new InlineResponse2001();
            resp.Profile = ConvertProfile(profile);
            return StatusCode(200, resp);
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2001));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
        }

        /// <summary>
        /// Get a profile
        /// </summary>
        /// <remarks>Get a profile of a user of the system. Auth is optional</remarks>
        /// <param name="username">Username of the profile to get</param>
        /// <response code="200">Profile</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpGet]
        [Route("/api/profiles/{username}")]
        [ValidateModelState]
        [SwaggerOperation("GetProfileByUsername")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse2001), description: "Profile")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> GetProfileByUsername([FromRoute][Required]string username)
        {
            int? userid = tokenizer.GetIdFromAuthedUser(User);
            if (userid == null)
            {
                Console.WriteLine("Authentication is optional");
            }
            var contract = new ProfileGetContract();
            contract.FollowedUsername = username;
            contract.UserId = userid.Value;
            var validator = new ProfileGetValidator(contract);
            if (!validator.Validate())
            {
                Console.WriteLine("WebHost.GetProfile validation failure: " + validator.GetError().ToString());
                return StatusCode(422, default(GenericErrorModel));
            }
            ProfileGetResponseContract response = await caller.GetProfile(contract);
            if (response == null)
            {
                return StatusCode(422, default(GenericErrorModel));
            }
            InlineResponse2001 resp = new InlineResponse2001();
            resp.Profile = ConvertProfile(response);
            return StatusCode(200, resp);
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse2001));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
        }
    }
}

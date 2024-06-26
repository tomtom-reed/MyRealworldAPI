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
using System.ComponentModel.DataAnnotations;
using RealworldApi.Web.Attributes;
using RealworldApi.Web.Security;
using Microsoft.AspNetCore.Authorization;
using RealworldApi.Web.Models;
using RealworldWeb.Caller;
using Contracts.Communicator.Request;
using Contracts.Validators;

namespace RealworldApi.Web.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class FavoritesApiController : ControllerBase
    {
        private ITokenUtils tokenizer;
        private IFavoriteCaller caller;
        public FavoritesApiController(ITokenUtils tokenizer, IFavoriteCaller caller)
        {
            this.tokenizer = tokenizer;
            this.caller = caller;
        }
        /// <summary>
        /// Favorite an article
        /// </summary>
        /// <remarks>Favorite an article. Auth is required</remarks>
        /// <param name="slug">Slug of the article that you want to favorite</param>
        /// <response code="200">Single article</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/articles/{slug}/favorite")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("CreateArticleFavorite")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse201), description: "Single article")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> CreateArticleFavorite([FromRoute][Required]string slug)
        {
            int? userid = tokenizer.GetIdFromAuthedUser(User);
            if (userid == null)
            {
                Console.WriteLine("Authentication must have failed");
                return StatusCode(401);
            }
            ProfileFavoriteContract contract = new ProfileFavoriteContract();
            contract.ArticleSlug = slug;
            contract.UserId = userid.Value;
            ProfileFavoriteValidator validator = new ProfileFavoriteValidator(contract);
            if (!validator.Validate())
            {
                Console.WriteLine("Validation failed");
                return StatusCode(422);
            }
            var response = await caller.FavoriteArticle(contract);
            if (response == null) {
                return StatusCode(422, default(GenericErrorModel));
            }
            InlineResponse201 resp = new InlineResponse201();
            resp.Article = ArticlesApiController.ArticleContractToArticleResponse(response);
            return StatusCode(200, resp);

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse201));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
        }

        /// <summary>
        /// Unfavorite an article
        /// </summary>
        /// <remarks>Unfavorite an article. Auth is required</remarks>
        /// <param name="slug">Slug of the article that you want to unfavorite</param>
        /// <response code="200">Single article</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpDelete]
        [Route("/api/articles/{slug}/favorite")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
        [ValidateModelState]
        [SwaggerOperation("DeleteArticleFavorite")]
        [SwaggerResponse(statusCode: 200, type: typeof(InlineResponse201), description: "Single article")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> DeleteArticleFavorite([FromRoute][Required]string slug)
        {
            int? userid = tokenizer.GetIdFromAuthedUser(User);
            if (userid == null)
            {
                Console.WriteLine("Authentication must have failed");
                return StatusCode(401);
            }
            ProfileFavoriteContract contract = new ProfileFavoriteContract();
            contract.ArticleSlug = slug;
            contract.UserId = userid.Value;
            ProfileFavoriteValidator validator = new ProfileFavoriteValidator(contract);
            if (!validator.Validate())
            {
                Console.WriteLine("Validation failed");
                return StatusCode(422);
            }
            var response = await caller.UnfavoriteArticle(contract);
            if (response == null)
            {
                return StatusCode(422, default(GenericErrorModel));
            }
            InlineResponse201 resp = new InlineResponse201();
            resp.Article = ArticlesApiController.ArticleContractToArticleResponse(response);
            return StatusCode(200, resp);
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse201));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
        }
    }
}

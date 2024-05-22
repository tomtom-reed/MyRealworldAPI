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

namespace RealworldApi.Web.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class FavoritesApiController : ControllerBase
    { 
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
        public virtual IActionResult CreateArticleFavorite([FromRoute][Required]string slug)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse201));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
            string exampleJson = null;
            exampleJson = "{\n  \"article\" : {\n    \"tagList\" : [ \"tagList\", \"tagList\" ],\n    \"createdAt\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"author\" : {\n      \"image\" : \"image\",\n      \"following\" : true,\n      \"bio\" : \"bio\",\n      \"username\" : \"username\"\n    },\n    \"description\" : \"description\",\n    \"title\" : \"title\",\n    \"body\" : \"body\",\n    \"favoritesCount\" : 0,\n    \"slug\" : \"slug\",\n    \"updatedAt\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"favorited\" : true\n  }\n}";
            
                        var example = exampleJson != null
                        ? JsonSerializer.Deserialize<InlineResponse201>(exampleJson)
                        : default(InlineResponse201);            //TODO: Change the data returned
            return new ObjectResult(example);
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
        public virtual IActionResult DeleteArticleFavorite([FromRoute][Required]string slug)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(InlineResponse201));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
            string exampleJson = null;
            exampleJson = "{\n  \"article\" : {\n    \"tagList\" : [ \"tagList\", \"tagList\" ],\n    \"createdAt\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"author\" : {\n      \"image\" : \"image\",\n      \"following\" : true,\n      \"bio\" : \"bio\",\n      \"username\" : \"username\"\n    },\n    \"description\" : \"description\",\n    \"title\" : \"title\",\n    \"body\" : \"body\",\n    \"favoritesCount\" : 0,\n    \"slug\" : \"slug\",\n    \"updatedAt\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"favorited\" : true\n  }\n}";
            
                        var example = exampleJson != null
                        ? JsonSerializer.Deserialize<InlineResponse201>(exampleJson)
                        : default(InlineResponse201);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
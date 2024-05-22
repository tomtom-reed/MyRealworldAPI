using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace RealworldApi.Web.Security
{

    public interface ITokenUtils
    {
        string GetToken(string email);
    }
    public class TokenUtils : ITokenUtils
    {
        // https://dotnetcorecentral.com/blog/authentication-handler-in-asp-net-core/
        private readonly byte[] tokenKey;

        public TokenUtils(byte[] tokenKey)
        {
            //Console.WriteLine("TokenUtils constructor, tokenKey = " + Encoding.UTF8.GetString(tokenKey));
            this.tokenKey = tokenKey;
        }

        public string GetToken(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email)
                }),
                Expires = DateTime.UtcNow.AddHours(12), // TODO reduce to 1
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
    /// <summary>
    /// class to handle api_key security.
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// scheme name for authentication handler.
        /// </summary>
        public const string SchemeName = "ApiKey";
        private readonly byte[] TokenKey;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            //Console.WriteLine("ApiKeyAuthenticationHandler constructor");
            TokenKey = Convert.FromHexString(configuration.GetValue<string>("JWT:KeyHex")); // checked in Program.cs
        }

        /// <summary>
        /// verify that require api key header exist and handle authorization.
        /// </summary>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // REF
            // https://dev.to/kazinix/aspnet-core-custom-token-authentication-2j9a
            // https://dotnetcorecentral.com/blog/authentication-handler-in-asp-net-core/

            if (!Request.Headers.ContainsKey("api_key"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(Request.Headers["api_key"]);
            // TODO Validate the key and all that 
            /*if (token.SigningCredentials.Algorithm != SecurityAlgorithms.HmacSha256Signature
                || !token.SigningKey.Equals(Encoding.ASCII.GetBytes(ApiKeyAuthenticationHandler.TokenKey)))
            {
                Console.WriteLine("Failed to verify token");
                return AuthenticateResult.Fail("Failed to verify token");
            }*/
            List<Claim> claims = token.Claims.ToList();

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // TODO
            // I'm going to set this aside for now, I think it's working. 
            // The canonical way of doing this MIGHT be
            //      builder.Services.AddHttpContextAccessor()
            //      builder.Services.AddTransient<ClaimsPrincipal>(.....)
            // public Controller(ClaimsPrincipal user)
            // this "unique_name" is just a workaround that might be working for now. 

            return AuthenticateResult.Success(ticket);
        }
    }
}

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
        string GetToken(int userId);
        int? GetIdFromAuthedUser(ClaimsPrincipal user);
        ClaimsPrincipal? ValidateToken(string token);
    }
    public class TokenUtils : ITokenUtils
    {
        // https://dotnetcorecentral.com/blog/authentication-handler-in-asp-net-core/
        //private readonly byte[] tokenKey;
        SymmetricSecurityKey cryptokey;
        SymmetricSecurityKey signingkey;

        public TokenUtils(SymmetricSecurityKey encryptKey, SymmetricSecurityKey signingkey)
        {
            //Console.WriteLine("TokenUtils constructor, tokenKey = " + Encoding.UTF8.GetString(tokenKey));
            this.cryptokey = encryptKey;
            this.signingkey = signingkey;
            //TokenKey = Convert.FromHexString(configuration.GetValue<string>("JWT:KeyHex")); // checked in Program.cs
            this.signingkey = signingkey;
        }

        public int? GetIdFromAuthedUser(ClaimsPrincipal user)
        {
            if (user == null || user.Claims == null)
            {
                return null;
            }
            try
            {
                if (user.Claims.FirstOrDefault(c => c.Type == "id") is Claim claim)
                {
                    return int.Parse(claim.Value);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Parsing UserID failed: " + ex.Message);
            }
            return null;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "MyIssuer",

                ValidAudience = "MyAudience",
                ValidateAudience = true,
                RequireAudience = true,

                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,

                IssuerSigningKey = cryptokey, // TODO shouldn't use the same credentials for both signing and encryption
                TokenDecryptionKey = cryptokey,
                ValidateIssuerSigningKey = true,
                ValidateTokenReplay = true,
                ValidateActor = true,
                RequireSignedTokens = true,
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
                //return GetIdFromAuthedUser(principal);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to validate token: " + e.Message);
                return null;
            }
        }

        public string GetToken(int userid)
        {
            if (userid < 0)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "MyAudience",
                Issuer = "MyIssuer",

                Claims = new Dictionary<string, object>
                {
                    ["id"] = userid,
                },

                Expires = DateTime.UtcNow.AddHours(12), // TODO reduce to 1
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,

                SigningCredentials = new SigningCredentials(
                    cryptokey,
                    SecurityAlgorithms.HmacSha256Signature),
                EncryptingCredentials = new EncryptingCredentials(
                    cryptokey,
                    SecurityAlgorithms.Aes128KW,
                    SecurityAlgorithms.Aes128CbcHmacSha256)
            };
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwtSecurityToken);
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
        private ITokenUtils tokenUtils;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IConfiguration configuration,
            ITokenUtils tokenUtils) : base(options, logger, encoder, clock)
        {
            //Console.WriteLine("ApiKeyAuthenticationHandler constructor");
            this.tokenUtils = tokenUtils;
            
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

            var principal = tokenUtils.ValidateToken(Request.Headers["api_key"]);
            if (principal == null)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
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

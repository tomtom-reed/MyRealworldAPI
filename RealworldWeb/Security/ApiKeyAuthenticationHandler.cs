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
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RealworldWeb.Utils;

namespace RealworldApi.Web.Security
{

    public interface ITokenUtils
    {
        string GetToken(int userId);
        int? GetIdFromAuthedUser(ClaimsPrincipal user);
        Task<ClaimsPrincipal?> ValidateToken(string token);
    }
    public class TokenUtils : ITokenUtils
    {
        // https://dotnetcorecentral.com/blog/authentication-handler-in-asp-net-core/
        //private readonly byte[] tokenKey;
        private RsaSecurityKey cryptokey_public;
        private RsaSecurityKey cryptokey_private;
        private SymmetricSecurityKey signingkey;
        private int expirationHours;

        public TokenUtils(WebConfiguration configuration)
        {
            //Console.WriteLine("TokenUtils constructor, tokenKey = " + Encoding.UTF8.GetString(tokenKey));
            this.cryptokey_public = configuration.JWT_CryptoKey_Public;
            this.cryptokey_private = configuration.JWT_CryptoKey_Private;
            this.signingkey = configuration.JWT_SigningKey;
            this.expirationHours = configuration.JWT_ExpirationHours;
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

        public async Task<ClaimsPrincipal?> ValidateToken(string token)
        {
            //Console.WriteLine("Validating token: " + token);
            var tokenHandler = new JsonWebTokenHandler();
            //JsonWebTokenHandler jwthandler = new JsonWebTokenHandler();
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

                //IssuerSigningKey = signingkey,
                TokenDecryptionKey = cryptokey_private,
                RequireSignedTokens = false, // DO NOT DO THIS
                //ValidateIssuerSigningKey = true,
                ValidateTokenReplay = false, // TODO? 
                ValidateActor = true,
                //RequireSignedTokens = true,
            };
            try
            {
                //var parsedToken = tokenHandler.ReadToken(token);
                var principal = await tokenHandler.ValidateTokenAsync(token, validationParameters);
                /*foreach (var claim in principal.Claims)
                {
                    Console.WriteLine("Claim: " + claim.Key + " = " + claim.Value);
                }*/
                return new ClaimsPrincipal(principal.ClaimsIdentity);
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
            var tokenHandler = new JsonWebTokenHandler(); //JwtSecurityTokenHandler
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "MyAudience",
                Issuer = "MyIssuer",

                //Claims = new Dictionary<string, object>
                //{
                //    ["id"] = userid,
                //},

                Subject = new ClaimsIdentity(new List<Claim> { new Claim("id", userid.ToString()) }),

                Expires = DateTime.UtcNow.AddHours(expirationHours),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,

                //SigningCredentials = new SigningCredentials(
                //    signingkey,
                //    SecurityAlgorithms.HmacSha256Signature),
                //EncryptingCredentials = new EncryptingCredentials(
                //    cryptokey,
                //    SecurityAlgorithms.Aes256Gcm,
                //    SecurityAlgorithms.Aes128CbcHmacSha256)
                EncryptingCredentials = new EncryptingCredentials(
                    cryptokey_public, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512)
            };
            //var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            //var jwtSecurityToken = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.CreateEncodedJwt(tokenDescriptor);
            //return tokenHandler.WriteToken(jwtSecurityToken);
            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
    /// <summary>
    /// class to handle Authentication Token security.
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

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            string[] authheader = Request.Headers["Authorization"].ToString().Split(" "); 
            if (authheader.Length != 2 || authheader[0] != "Token")
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            var principal = await tokenUtils.ValidateToken(authheader[1]);
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

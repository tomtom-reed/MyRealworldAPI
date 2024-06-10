using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using RealworldApi.Web.Security;
using RealworldWeb.Caller;
using RealworldWeb.Utils;
using System.Text;

namespace RealworldAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            /* 
             * Auth Service setup and config validation 
             */
            /*string? jwtCryptoHex = builder.Configuration.GetValue<string>("JWT:CryptoKey");
            SymmetricSecurityKey jwtCryptoKey;
            if (jwtCryptoHex == null || jwtCryptoHex.Length != 32)
            {
                Console.WriteLine("Appsettings failure: JWT crypto key must be exactly 64 hex characters");
                return;
            }
            try
            {
                Convert.FromHexString(jwtCryptoHex);
                jwtCryptoKey = new SymmetricSecurityKey(Convert.FromHexString(jwtCryptoHex));
            } catch (Exception ex)
            {
                Console.WriteLine("Appsettings failure at JWT crypto key convert: " + ex.Message);
                return;
            }*/

            /*string? jwtSigningHex = builder.Configuration.GetValue<string>("JWT:CryptoKey");
            SymmetricSecurityKey jwtSigningKey;
            if (jwtSigningHex == null || jwtSigningHex.Length != 32)
            {
                Console.WriteLine("Appsettings failure: JWT signing key must be exactly 64 hex characters");
                return;
            }
            try
            {
                Convert.FromHexString(jwtSigningHex);
                jwtSigningKey = new SymmetricSecurityKey(Convert.FromHexString(jwtSigningHex));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Appsettings failure at JWT signing key convert: " + ex.Message);
                return;
            }*/


            builder.Services.AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationHandler.SchemeName, options => { });
            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });


            /*string? webhostUrl = builder.Configuration.GetValue<string>("Connections:WebHost");
            if (string.IsNullOrEmpty(webhostUrl))
            {
                Console.WriteLine("Appsettings failure at Connections:WebHost");
                return;
            }*/

            /*
             * Singleton Setup
             * */
            builder.Services.AddSingleton(new WebConfiguration(builder.Configuration));
            builder.Services.AddSingleton<ITokenUtils, TokenUtils>();
            builder.Services.AddSingleton<IUserCaller, UserCaller>();
            builder.Services.AddSingleton<IArticleCaller, ArticleCaller>();
            builder.Services.AddSingleton<ICommentCaller, CommentCaller>();
            //builder.Services.AddSingleton<ITagCaller, TagCaller>();
            builder.Services.AddSingleton<IProfileCaller, ProfileCaller>();
            builder.Services.AddSingleton<IFavoriteCaller, FavoriteCaller>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

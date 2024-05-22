using Microsoft.AspNetCore.Authentication;
using RealworldApi.Web.Security;
using RealworldWeb.Caller;
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
            string? tokenKey = builder.Configuration.GetValue<string>("JWT:KeyHex");
            if (tokenKey == null || tokenKey.Length != 32)
            {
                Console.WriteLine("Appsettings failure: JWT Token must be exactly 64 hex characters");
                return;
            }
            try
            {
                Convert.FromHexString(tokenKey);
            } catch (Exception ex)
            {
                Console.WriteLine("Appsettings failure at JWT Token convert: " + ex.Message);
                return;
            }
            builder.Services.AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationHandler.SchemeName, options => { });
            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });


            string? webhostUrl = builder.Configuration.GetValue<string>("Connections:WebHost");
            if (string.IsNullOrEmpty(webhostUrl))
            {
                Console.WriteLine("Appsettings failure at Connections:WebHost");
                return;
            }

            /*
             * Singleton Setup
             * */
            builder.Services.AddSingleton<ITokenUtils>(new TokenUtils(Convert.FromHexString(tokenKey)));
            builder.Services.AddSingleton<IUserCaller>(new UserCaller(webhostUrl));

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

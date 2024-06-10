using Microsoft.Data.SqlClient;
using RealworldWebHost.DataAccess;
using RealworldWebHost.Utilities;

namespace RealworldWebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /*
             * Configure 
             */
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Raw connection string from Server Explorer:
            // Data Source=localhost\SQLEXPRESS;Initial Catalog=RealworldDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True
            // If we're using the "qadb" user in SSDT then we need to tinker a few things. 
            string? datasource_Url = builder.Configuration.GetValue<string>("Datasource:Url");
            string? datasource_UserId = builder.Configuration.GetValue<string>("Datasource:UserID");
            string? datasource_Password = builder.Configuration.GetValue<string>("Datasource:Password");
            string? datasource_Catalog = builder.Configuration.GetValue<string>("Datasource:Catalog");
            if (string.IsNullOrEmpty(datasource_Url)
                || string.IsNullOrEmpty(datasource_UserId)
                || string.IsNullOrEmpty(datasource_Password)
                || string.IsNullOrEmpty(datasource_Catalog))
            {
                Console.WriteLine("WebHost Config failure in Datasource");
                return;
            }
            SqlConnectionStringBuilder scsbuilder = new SqlConnectionStringBuilder();
            scsbuilder.DataSource = datasource_Url;
            scsbuilder.UserID = datasource_UserId;
            scsbuilder.Password = datasource_Password;
            scsbuilder.InitialCatalog = datasource_Catalog;
            // Bad practice, do not do this in prod 
            scsbuilder.TrustServerCertificate = true;
            scsbuilder.Encrypt = false;
            scsbuilder.IntegratedSecurity = false;
            scsbuilder.Authentication = SqlAuthenticationMethod.SqlPassword;

            /*
             * Vault mock
             */
            string? aes_str = builder.Configuration.GetValue<string>("Vault:AES_HEX");
            if (aes_str == null || aes_str.Length != 32)
            {
                Console.WriteLine("Appsettings failure: AES_KEY is invalid"); 
                return;
            }
            byte[] aes_key;
            try
            {
                aes_key = Convert.FromHexString(aes_str);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Appsettings failure at AES_KEY convert from hex: " + ex.Message);
                return;
            }



            /* 
             * Singleton Setup 
             */
            builder.Services.AddSingleton<ISecUtils>(new SecUtils(aes_key));
            builder.Services.AddSingleton<IArticleDA>(s =>
            {
                var sec = (ISecUtils)s.GetRequiredService(typeof(ISecUtils));
                return new ArticleDA(sec, scsbuilder.ConnectionString);
            });
            builder.Services.AddSingleton<IUserDA>(s =>
            {
                var sec = (ISecUtils)s.GetRequiredService(typeof(ISecUtils));
                return new UserDA(sec, scsbuilder.ConnectionString);
            });
            builder.Services.AddSingleton<ICommentDA>(new CommentDA(scsbuilder.ConnectionString));
            builder.Services.AddSingleton<IProfileDA>(new ProfileDA(scsbuilder.ConnectionString));


            /*
             * Build App
             */
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

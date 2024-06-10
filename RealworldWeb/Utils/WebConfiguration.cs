using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace RealworldWeb.Utils
{
    public class WebConfiguration
    {
        //public readonly string AllowedHosts;

        public readonly string Connections_WebHost = "";

        public readonly int JWT_ExpirationHours = 0;
        public readonly RsaSecurityKey JWT_CryptoKey_Public;
        public readonly RsaSecurityKey JWT_CryptoKey_Private;
        public readonly SymmetricSecurityKey JWT_SigningKey;

        // Logging? 

        public WebConfiguration(IConfiguration configuration)
        {
            //AllowedHosts = configuration.GetValue<string>("AllowedHosts");
            string? webHostUrl = configuration.GetValue<string>("Connections:WebHost");
            if (string.IsNullOrEmpty(webHostUrl))
            {
                Console.WriteLine("Appsettings failure at Connections:WebHost");
                throw new Exception("Appsettings failure at Connections:WebHost");
            }
            this.Connections_WebHost = webHostUrl;

            JWT_ExpirationHours = configuration.GetValue<int>("JWT:ExpirationHours");
            if (JWT_ExpirationHours <= 0)
            {
                Console.WriteLine("Appsettings failure at JWT:ExpirationHours");
                throw new Exception("Appsettings failure at JWT:ExpirationHours");
            }

            // Crypto Key
            string? jwtCryptoHex = configuration.GetValue<string>("JWT:CryptoKey");
            if (jwtCryptoHex == null || jwtCryptoHex.Length != 64)
            {
                Console.WriteLine("Appsettings failure: JWT crypto key must be exactly 64 hex characters");
                throw new Exception("JWT crypto key must be exactly 64 hex characters");
            }
            try
            {
                //JWT_CryptoKey = new SymmetricSecurityKey(Convert.FromHexString(jwtCryptoHex));
                RSA rsa = RSA.Create(3072);
                JWT_CryptoKey_Private = new RsaSecurityKey(rsa) { KeyId = jwtCryptoHex };
                JWT_CryptoKey_Public = new RsaSecurityKey(rsa.ExportParameters(false)) { KeyId = jwtCryptoHex };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Appsettings failure at JWT crypto key convert: " + ex.Message);
                throw;
            }

            // Signing Key
            string? jwtSigningHex = configuration.GetValue<string>("JWT:CryptoKey");
            if (jwtSigningHex == null || jwtSigningHex.Length != 64)
            {
                Console.WriteLine("Appsettings failure: JWT signing key must be exactly 64 hex characters");
                throw new Exception("JWT signing key must be exactly 64 hex characters");
            }
            try
            {
                Convert.FromHexString(jwtSigningHex);
                JWT_SigningKey = new SymmetricSecurityKey(Convert.FromHexString(jwtSigningHex));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Appsettings failure at JWT signing key convert: " + ex.Message);
                throw;
            }
        }
    }
}

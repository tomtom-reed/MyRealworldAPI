﻿using NSec.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace RealworldWebHost.Utilities
{
    public interface ISecUtils
    {
        bool ComparePasswordToHash(string password, byte[] hashedPassword);
        byte[] HashPassword(string password);
        byte[] HashSha256(string input); 
        byte[] Encrypt(string cleartext);
        string Decrypt(byte[] ciphertext);


    }
    public class SecUtils : ISecUtils
    {
        private const int IV_LEN = 16;
        private const int PW_SALT_LEN = 16;
        private const int PW_HASH_LEN = 64;
        private readonly byte[] AES_KEY; 

        public SecUtils(byte[] key)
        {
            this.AES_KEY = key;
        }

        private byte[] GetIV()
        {
            return GetRandomBytes(IV_LEN);
        }

        private byte[] GetRandomBytes(int len)
        {
            if (len < 0)
            {
                return new byte[0];
            }
            Byte[] b = new byte[len];
            Random rnd = new Random();
            rnd.NextBytes(b);
            return b;
        }

        public bool ComparePasswordToHash (string password, byte[] hashedPassword)
        {
            // Check 1: Hashed passwords are concat(hash, salt). Length needs to be exact. 
            if (hashedPassword.Length != PW_HASH_LEN + PW_SALT_LEN) {
                Console.WriteLine("PW Compare bad length of stored password");
                return false; 
            }

            // Hash the provided password. If they match, return true; 
            var pw1 = hashedPassword[..PW_HASH_LEN];
            var salt = hashedPassword[PW_HASH_LEN..];

            var pw2 = HashPassword(password, salt);
            //if (Array.Equals(hashedPassword, pw2)) {
            if(Enumerable.SequenceEqual(hashedPassword, pw2)) { 
                return true; 
            }
            Console.WriteLine("Array.Equals on passwords failed");
            return false;
        }

        /// <summary>
        /// Returns a salted password concatenated with its salt. 
        /// This is safe to store in the DB and can be provided to ComparePasswordToHash. 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public byte[] HashPassword(string password)
        {
            var salt = GetRandomBytes(PW_SALT_LEN);
            return HashPassword(password, salt);
        }

        /// <summary>
        /// Generates a password hash from the given password and salt. 
        /// The response is always exactly HASH_LEN bytes (default 64). 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private byte[] HashPassword(string password, byte[] salt)
        {
            // https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
            
            var hashParams = new Argon2Parameters();
            hashParams.DegreeOfParallelism = 1;
            hashParams.NumberOfPasses = 2;
            hashParams.MemorySize = 19456; // 19mb?

            var argon2 = PasswordBasedKeyDerivationAlgorithm.Argon2id(hashParams);

            var hash = argon2.DeriveBytes(password, salt, PW_HASH_LEN);
            return hash.Concat(salt).ToArray();
        }

        /// <summary>
        /// Hashes an input string with SHA256. Unsalted. Always produces 32 bytes (256 bits).
        /// Use this to salt emails for safe but searchable storage. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] HashSha256 (string input)
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(input));
        }

        #region crypt
        // https://learn.microsoft.com/en-us/dotnet/standard/security/encrypting-data
        // https://code-maze.com/csharp-string-encryption-decryption/

        // if my memory serves me, standard practice for IVs is to append them to the ciphertext

        // So the byte[] will be appended with an IV of length IV_LEN generated by renamed GetSalt
        // And for decrypt we just use the last 16 bytes of the ciphertext as the IV and decrypt the rest 

        public byte[] Encrypt(string cleartext)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AES_KEY;
                    aes.IV = GetIV();
                    // TODO set the IV 
                    //byte[] iv = aes.IV; // Possible issue here 
                    using (MemoryStream output = new())
                    {
                        using (CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(Encoding.Unicode.GetBytes(cleartext));
                            cryptoStream.FlushFinalBlock();
                            output.Write(aes.IV);
                            return output.ToArray();
                        }
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return new byte[0];
        }

        public string Decrypt(byte[] ciphertext)
        {
            // Check 1: Hashed passwords are concat(hash, salt). Length needs to be exact. 
            if (ciphertext.Length <= IV_LEN) { return null; }

            // Get the parts
            var cipher = ciphertext[..(ciphertext.Length - IV_LEN)];
            var IV = ciphertext[(ciphertext.Length - IV_LEN)..];

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AES_KEY;
                    aes.IV = IV;
                    
                    byte[] iv = aes.IV; // Possible issue here 
                    using (MemoryStream input = new(cipher))
                    {
                        using (CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (MemoryStream output = new())
                            {
                                cryptoStream.CopyTo(output);
                                return Encoding.Unicode.GetString(output.ToArray());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
        #endregion crypt
    }
}

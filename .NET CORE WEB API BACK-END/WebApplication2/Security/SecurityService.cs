using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication2.Interfaces;

namespace WebApplication2.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly byte[] _key;

        public SecurityService(string ConnString, byte[] salt)
        {
            _key = DeriveKeyFromPassword(ConnString, salt);
        }

        public byte[] DeriveKeyFromPassword(string ConnString, byte[] salt)
        {
            var iterations = 1000;
            var desiredKeyLength = 32;
            var hashMethod = HashAlgorithmName.SHA256;

            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(ConnString), salt, iterations, hashMethod))
            {
                return deriveBytes.GetBytes(desiredKeyLength);
            }
        }

        public async Task<string> DecryptAsync(string cipherText)
        {
            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);
                using (var aes = Aes.Create())
                {
                    aes.Key = _key;

                    byte[] iv = new byte[aes.BlockSize / 8];
                    byte[] cipher = new byte[fullCipher.Length - iv.Length];

                    Array.Copy(fullCipher, iv, iv.Length);
                    Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                    aes.IV = iv;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var descriptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(cipher))
                    using (var cs = new CryptoStream(ms, descriptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return await sr.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption failed: " + ex.Message);
                throw;
            }
        }

        public string GenerateWebToken(string key, string userName, int expireMinutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

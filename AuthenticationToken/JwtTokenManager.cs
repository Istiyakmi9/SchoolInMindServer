using AuthenticationToken.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UtilityService;

namespace AuthenticationToken
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly JwtSetting _jwtSetting;
        public JwtTokenManager(JwtSetting jwtSetting)
        {
            _jwtSetting = jwtSetting;
        }

        public SessionModal ReadJwtToken(string AuthorizationToken)
        {
            SessionModal sessionModal = new SessionModal();
            if (!string.IsNullOrEmpty(AuthorizationToken))
            {
                string token = AuthorizationToken.Replace("Bearer", "").Trim();
                if (!string.IsNullOrEmpty(token) && token != "null")
                {
                    var handler = new JwtSecurityTokenHandler();
                    handler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _jwtSetting.Issuer,
                        ValidAudience = _jwtSetting.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key))
                    }, out SecurityToken validatedToken);

                    var securityToken = handler.ReadToken(token) as JwtSecurityToken;
                    sessionModal.UserId = securityToken.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
                    sessionModal.TenentId = securityToken.Claims.FirstOrDefault(x => x.Type == "TenentId").Value;
                    sessionModal.Mobile = securityToken.Claims.FirstOrDefault(x => x.Type == "Mobile").Value;
                    sessionModal.Email = securityToken.Claims.FirstOrDefault(x => x.Type == "Email").Value;
                    sessionModal.Role = securityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;
                }
            }
            return sessionModal;
        }

        public string GenerateToken<T>(T ClaimArguments)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            Type type = typeof(T);
            foreach (var prop in type.GetProperties())
            {
                var Data = TypeConverter.ConvertTo(prop.GetValue(ClaimArguments), prop.PropertyType);
                claims.Add(new Claim(prop.Name, Data));
            }

            var token = new JwtSecurityToken(_jwtSetting.Issuer,
              _jwtSetting.Issuer,
              claims: claims.ToArray(),
              expires: DateTime.UtcNow.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateToken<T>(string Subject, string Email, T ClaimArguments)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, Subject));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, Email ?? ""));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            if (ClaimArguments != null)
            {
                Type type = typeof(T);
                foreach (var prop in type.GetProperties())
                {
                    var Data = TypeConverter.ConvertTo(prop.GetValue(ClaimArguments), prop.PropertyType);
                    if (prop.Name.ToLower() == "role")
                        claims.Add(new Claim("role", Data ?? ""));
                    else
                        claims.Add(new Claim(prop.Name, Data ?? ""));
                }
            }

            var token = new JwtSecurityToken(_jwtSetting.Issuer,
              _jwtSetting.Issuer,
              claims: claims.ToArray(),
              expires: DateTime.UtcNow.AddMinutes(_jwtSetting.AccessTokenExpiryTimeInMinutes),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenModal GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokenModal
                {
                    RefreshToken = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddHours(_jwtSetting.RefreshTokenExpiryTimeInHours),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        private int GetSaltSize(byte[] passwordBytes)
        {
            var key = new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000);
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }
        private byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        public string Encrypt(string text, string pwd)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256  
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Getting the salt size  
            int saltSize = GetSaltSize(passwordBytes);
            // Generating salt bytes  
            byte[] saltBytes = GetRandomBytes(saltSize);

            // Appending salt bytes to original bytes  
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }
        public string Decrypt(string decryptedText, string pwd)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256  
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            if (decryptedBytes != null)
            {
                // Getting the size of salt  
                int saltSize = GetSaltSize(passwordBytes);

                // Removing salt bytes, retrieving original bytes  
                byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
                for (int i = saltSize; i < decryptedBytes.Length; i++)
                {
                    originalBytes[i - saltSize] = decryptedBytes[i];
                }
                return Encoding.UTF8.GetString(originalBytes);
            }
            else
            {
                return null;
            }
        }

        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:  
            byte[] saltBytes = passwordBytes;
            // Example:  
            //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };  

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            try
            {
                byte[] decryptedBytes = null;
                // Set your salt here to meet your flavor:  
                byte[] saltBytes = passwordBytes;
                // Example:  
                //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };  

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        //AES.Mode = CipherMode.CBC;  

                        using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            //If(cs.Length = ""  
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }
                return decryptedBytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

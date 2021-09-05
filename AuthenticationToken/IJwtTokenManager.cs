using AuthenticationToken.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationToken
{
    public interface IJwtTokenManager
    {
        SessionModal ReadJwtToken(string AuthorizationToken);
        string GenerateToken<T>(string Subject, string Email, T ClaimArguments);
        public string GenerateToken<T>(T ClaimArguments);
        public RefreshTokenModal GenerateRefreshToken(string ipAddress);
        public string Encrypt(string Text, string Password);
        public string Decrypt(string DecryptedText, string Password);
    }
}

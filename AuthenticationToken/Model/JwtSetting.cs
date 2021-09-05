using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationToken.Model
{
    public class JwtSetting
    {
        public string Key { set; get; }
        public string Issuer { get; set; }
        public string Sub { set; get; }
        public long AccessTokenExpiryTimeInMinutes { set; get; }
        public long RefreshTokenExpiryTimeInHours { set; get; }
    }
}

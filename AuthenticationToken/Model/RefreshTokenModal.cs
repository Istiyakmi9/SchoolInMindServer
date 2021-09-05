using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationToken.Model
{
    public class RefreshTokenModal
    {
        public string RefreshToken { set; get; }
        public DateTime Expires { set; get; }
        public DateTime Created { set; get; }
        public string CreatedByIp { set; get; }
    }
}

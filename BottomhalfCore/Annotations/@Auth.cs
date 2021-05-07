using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @Auth : Attribute
    {
        private string TokenName;
        private bool TokenCheckRequired;
        /// <summary>
        /// <para />TokenName: Enable Token base authentication. Provide name of token.
        /// <para />Server will use this token name to fetch your token value from request header and validate user.
        /// <para />To avoid token base Authentication on any controler use @NoCheck Annotation.
        /// <para />TokenCheckRequired: Pass false to avoid token check for current controller else true.
        /// </summary>
        public @Auth(string TokenName, bool TokenCheckRequired)
        {
            this.TokenName = TokenName;
            this.TokenCheckRequired = TokenCheckRequired;
        }
    }
}

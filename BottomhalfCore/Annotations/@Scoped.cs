using System;
using System.Collections.Generic;
using System.Text;

namespace BottomhalfCore.Annotations
{
    public class @Scoped : Attribute
    {
        /// <summary>
        /// <para />TokenName: Enable Token base authentication. Provide name of token.
        /// <para />Server will use this token name to fetch your token value from request header and validate user.
        /// <para />To avoid token base Authentication on any controler use @NoCheck Annotation.
        /// <para />TokenCheckRequired: Pass false to avoid token check for current controller else true.
        /// </summary>
        public @Scoped() { }
    }
}

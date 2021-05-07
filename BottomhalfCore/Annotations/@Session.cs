using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @SpringSession : Attribute
    {
        private readonly Boolean IsSessionEnabled;
        public SpringSession(Boolean IsSessionEnabled)
        {
            this.IsSessionEnabled = IsSessionEnabled;
        }
    }
}

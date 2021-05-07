using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @Scope : Attribute
    {
        private string scopeType;

        public string ScopeType
        {
            get { return scopeType; }

            set { scopeType = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @InjectValue : Attribute
    {
        public string StringValue { set; get; }
        public InjectValue(string Value) { this.StringValue = Value; }
    }
}

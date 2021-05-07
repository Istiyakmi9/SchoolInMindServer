using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @RequestParam : Attribute
    {
        public string value { set; get; }
        public string defaultValue { set; get; }
    }
}

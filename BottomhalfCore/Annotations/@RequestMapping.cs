using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @RequestMapping : Attribute
    {
        public string method { set; get; }
        public string value { set; get; }
    }
}

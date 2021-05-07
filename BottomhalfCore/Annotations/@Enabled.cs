using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @Enabled : Attribute
    {
        public int Order { set; get; }
        public @Enabled(int Order) { this.Order = Order; }
    }
}

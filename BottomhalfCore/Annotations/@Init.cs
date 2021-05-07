using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @Init : Attribute
    {
        public int OrderPriority { set; get; }
        public Init(int OrderPriority)
        {
            this.OrderPriority = OrderPriority;
        }

        public Init()
        {
        }
        public void Order(int OrderPriority)
        {
            this.OrderPriority = OrderPriority;
        }
    }
}

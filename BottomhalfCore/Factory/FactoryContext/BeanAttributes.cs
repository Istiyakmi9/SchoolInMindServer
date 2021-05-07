using IFactoryContext.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.FactoryContext
{
    public class BeanAttributes : IAttributes
    {
        public string id { set; get; }
        public string value { set; get; }
        public string dependencyCheck { set; get; }
        public string dependsOn { set; get; }
        public string factoryMethod { set; get; }
        public string factoryBeans { set; get; }
        public string className { set; get; }
        public string scope { set; get; }
        public string _ref { set; get; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

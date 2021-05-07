using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.FactoryContext
{
    public interface ApplicationContext
    {
        void CreatBeanInstances();
        object GetBean(string FullyQualifiedClassName);
        Object CreateObject(IList<object> constructorParam, string ClassName);
        Object CreateInternalObject(IList<string> constructorParam, string ClassName);
    }
}

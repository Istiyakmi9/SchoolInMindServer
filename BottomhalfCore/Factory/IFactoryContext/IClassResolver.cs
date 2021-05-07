using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.IFactoryContext
{
    public interface IClassResolver
    {
        IDictionary<string, Type> ResolveClassType(IList<string> BeanClassPathList);
    }
}

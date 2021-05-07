using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.IFactoryContext
{
    public interface IAssemblyHandler
    {
        List<Assembly> LoadNamedAssemblies(string AsmName, ref string BinDir);
    }
}

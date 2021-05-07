using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.IFactoryContext
{
    public interface INameSpaceHandler
    {
        List<string> ResolveNamespace(List<string> QualifiedNamelist);
        List<string> ConvertNameSpaceToList(string SplittedName);
        string ConvertToImplemented(string Name);
        string GetImplementedName(string GenericName);
        List<string> CreateStringNameSpace(List<string> SplittedData);
    }
}

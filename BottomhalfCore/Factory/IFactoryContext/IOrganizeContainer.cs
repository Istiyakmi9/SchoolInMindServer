using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFactoryContext.IFactoryContext
{
    public interface IOrganizeContainer
    {
        ConcurrentDictionary<string, GraphContainerModal> ReOrganizeContainer(IDictionary<string, TypeRefCollection> ClassTypeCollection);
    }
}

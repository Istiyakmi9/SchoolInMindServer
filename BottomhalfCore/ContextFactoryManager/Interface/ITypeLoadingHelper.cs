using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Interface
{
    public interface ITypeLoadingHelper<T>
    {
        List<ImplementedHirarchy> GetDeepBaseTypes(Type CurrentType, List<Assembly> assemblyList);
    }
}

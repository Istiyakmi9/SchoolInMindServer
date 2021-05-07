using BottomhalfCore.BottomhalfModel;
using System.Collections.Generic;

namespace BottomhalfCore.ContextFactoryManager.Interface
{
    interface ILoasTypeDetail<T>
    {
        Dictionary<string, TypeRefCollection> BuildClassInformation(List<AopDetail> aopDetailLst);
    }
}

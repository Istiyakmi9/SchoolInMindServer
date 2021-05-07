using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Interface
{
    public interface IFindClassAssets<T>
    {
        Dictionary<int, ParameterDetail> InspectMethods(Type CurrentType);
        ClassAnnotationDetail InspectAnnotations(Type CurrentType, TypeRefCollection refCollection);
        List<FieldAttributes> InspectAttributes(Type CurrentType);
    }
}

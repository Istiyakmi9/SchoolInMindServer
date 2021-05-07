using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.ContextFactoryManager.Interface;
using BottomhalfCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Code
{
    public class TypeLoadingHelper : ITypeLoadingHelper<TypeLoadingHelper>
    {
        /// <summary>GetDeepBaseTypes
        /// <para></para>
        /// </summary>
        public List<ImplementedHirarchy> GetDeepBaseTypes(Type CurrentType, List<Assembly> assemblyList)
        {
            List<ImplementedHirarchy> ObjImplementedHirarchyList = new List<ImplementedHirarchy>();
            ImplementedHirarchy ObjImplementedHirarchy = null;
            Type NewBaseType = CurrentType;
            NewBaseType = NewBaseType.BaseType;
            int Index = 0;
            try
            {
                Type FilteredType = null;
                var ImplementedInterfaceTypes = CurrentType.GetInterfaces();
                if (ImplementedInterfaceTypes != null && ImplementedInterfaceTypes.Count() > 0)
                {
                    foreach (Type ParentInterface in ImplementedInterfaceTypes)
                    {
                        FilteredType = null;
                        ObjImplementedHirarchy = new ImplementedHirarchy();
                        try
                        {
                            FilteredType = assemblyList.SelectMany(a => a.GetTypes()).Where(x => x.Name == ParentInterface.Name).FirstOrDefault();
                        }
                        catch (Exception)
                        {
                            FilteredType = null;
                        }
                        if (FilteredType != null)
                        {
                            ObjImplementedHirarchy.TypeName = FilteredType.Name;
                            ObjImplementedHirarchy.TypeFullName = FilteredType.FullName;
                            if (FilteredType.IsInterface)
                                ObjImplementedHirarchy.ImplementedType = "interface";
                            else if (FilteredType.IsAbstract)
                                ObjImplementedHirarchy.ImplementedType = "abstract";
                            else
                                ObjImplementedHirarchy.ImplementedType = "class";
                            ObjImplementedHirarchy.IndexOrder = ++Index;
                            ObjImplementedHirarchyList.Add(ObjImplementedHirarchy);
                        }
                    }
                }
                return ObjImplementedHirarchyList;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "GetDeepBaseTypes()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "GetDeepBaseTypes()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }
    }
}

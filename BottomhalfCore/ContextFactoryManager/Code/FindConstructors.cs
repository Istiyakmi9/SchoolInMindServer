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
    public class FindConstructors : IFindClassAssets<FindConstructors>
    {
        /// <summary>InspectConstructor
        /// <para></para>
        /// </summary>
        public Dictionary<int, ParameterDetail> InspectMethods(Type CurrentType)
        {
            ParameterInfo[] parameters = null;
            ParameterDetail ObjParameterDetail = null;
            AnnotationDefination ObjAnnotationDefination = null;
            Dictionary<int, ParameterDetail> CtorParams = new Dictionary<int, ParameterDetail>();
            List<string> TypeCollections = null;
            ParameterNameCollection ObjParameterNameCollection = null;
            List<string> GenericParameterTypeName = new List<string>();
            try
            {
                int index = 0;
                TypeCollections = new List<string>();
                foreach (var ctor in CurrentType.GetConstructors())
                {
                    ObjParameterDetail = new ParameterDetail();
                    parameters = ctor.GetParameters();
                    foreach (var param in parameters)
                    {
                        var NewType = param.ParameterType;
                        String FullNameSpace = NewType.AssemblyQualifiedName;
                        if (FullNameSpace != null)
                        {
                            if (FullNameSpace != null)
                            {
                                ObjParameterNameCollection = new ParameterNameCollection();
                                ObjParameterDetail.WrapperFullDescription = "";
                                if (FullNameSpace.IndexOf("`") != -1)
                                    ObjParameterNameCollection.Name = FullNameSpace;
                                else
                                    ObjParameterNameCollection.Name = FullNameSpace.Split(new char[] { ',' })[0];
                                ObjParameterNameCollection.ArgumentName = param.Name;
                                ObjParameterDetail.Parameters.Add(ObjParameterNameCollection);
                            }
                            else
                            {
                                throw new ApplicationException("Unable to resolve constructor paramter for class: " + NewType.Name);
                            }
                        }
                        else if (NewType.AssemblyQualifiedName == null)
                        {
                            if (NewType.IsGenericType)
                            {
                                if (NewType.AssemblyQualifiedName == null)
                                {
                                    if (NewType.GenericTypeArguments.Count() > 0)
                                        ObjParameterNameCollection = BuildParameter(NewType, ref GenericParameterTypeName);
                                    if (ObjParameterNameCollection != null)
                                        TypeCollections.Add(ObjParameterNameCollection.Name);
                                }
                            }
                            else
                            {
                                ObjParameterNameCollection = BuildParameter(NewType, ref GenericParameterTypeName);
                                if (ObjParameterNameCollection != null)
                                    TypeCollections.Add(ObjParameterNameCollection.Name);
                            }
                            ObjParameterDetail.WrapperFullDescription = "";
                            if (ObjParameterNameCollection == null)
                                ObjParameterNameCollection = new ParameterNameCollection { Name = NewType.Name };
                            ObjParameterDetail.Parameters.Add(ObjParameterNameCollection);
                        }
                    }

                    IEnumerable<Attribute> CtorAttributes = ctor.GetCustomAttributes();
                    foreach (var attr in CtorAttributes)
                    {
                        ObjAnnotationDefination = new AnnotationDefination();
                        ObjAnnotationDefination.AnnotationName = attr.GetType().Name;
                        ObjAnnotationDefination.AppliedOn = "ctor";
                        if (ObjAnnotationDefination.AnnotationName == "Enabled")
                            ObjAnnotationDefination.OrderNo = ((BottomhalfCore.Annotations.Enabled)attr).Order;
                        if (ObjAnnotationDefination.AnnotationName == "File")
                            ObjAnnotationDefination.Filename = ((BottomhalfCore.Annotations.File)attr).Filename;
                        if (ObjParameterDetail.AnnotationDetail == null)
                            ObjParameterDetail.AnnotationDetail = new List<AnnotationDefination>();
                        ObjParameterDetail.AnnotationDetail.Add(ObjAnnotationDefination);
                    }

                    if ((ObjParameterDetail.AnnotationDetail != null && ObjParameterDetail.AnnotationDetail.Count > 0) || ObjParameterDetail.Parameters.Count > 0)
                        CtorParams.Add(++index, ObjParameterDetail);
                }
                if (TypeCollections.Count() > 0)
                {
                    ObjParameterDetail.IsGeneric = true;
                    if (GenericParameterTypeName.Count() > 0)
                        ObjParameterDetail.GenericParameterTypeName = GenericParameterTypeName;
                    else
                        GenericParameterTypeName = null;
                }

                return CtorParams;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "InspectConstructor()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "InspectConstructor()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>BuildParameter
        /// <para></para>
        /// </summary>
        private ParameterNameCollection BuildParameter(Type GenericClassType, ref List<string> GenericParameterTypeName)
        {
            ParameterNameCollection ObjGenericTypeDefination = null;
            try
            {
                if (GenericClassType.GenericTypeArguments.Count() > 0)
                {
                    ObjGenericTypeDefination = new ParameterNameCollection();
                    ObjGenericTypeDefination.Name = GenericClassType.Name;
                    foreach (Type InnerType in GenericClassType.GenericTypeArguments)
                    {
                        if (ObjGenericTypeDefination.ParameterStructurInfo == null)
                            ObjGenericTypeDefination.ParameterStructurInfo = new List<ParameterNameCollection>();
                        var InnerTypeDefination = BuildParameter(InnerType, ref GenericParameterTypeName);
                        ObjGenericTypeDefination.ParameterStructurInfo.Add(InnerTypeDefination);
                    }
                }
                else
                {
                    ObjGenericTypeDefination = new ParameterNameCollection();
                    ObjGenericTypeDefination.Name = GenericClassType.Name;
                    if (GenericParameterTypeName.Where(x => x == GenericClassType.Name).FirstOrDefault() == null)
                        GenericParameterTypeName.Add(GenericClassType.Name);
                }
                return ObjGenericTypeDefination;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "BuildParameter()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "BuildParameter()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        #region CURRENT INTERFACE MARKUP DECLARATION

        public ClassAnnotationDetail InspectAnnotations(Type CurrentType, TypeRefCollection refCollection)
        {
            throw new NotImplementedException();
        }

        public List<BottomhalfModel.FieldAttributes> InspectAttributes(Type CurrentType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.ContextFactoryManager.Interface;
using BottomhalfCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BottomhalfCore.ContextFactoryManager.Code
{
    public class FindAttributes : IFindClassAssets<FindAttributes>
    {
        /// <summary>InspectAttributes
        /// <para></para>
        /// </summary>
        public List<BottomhalfModel.FieldAttributes> InspectAttributes(Type CurrentType)
        {
            List<BottomhalfModel.FieldAttributes> ObjFieldAttributesCollection = new List<BottomhalfModel.FieldAttributes>();
            IEnumerable<Attribute> FAttributes = null;
            BottomhalfModel.FieldAttributes ObjFieldAttributes = null;
            try
            {
                var Fields = CurrentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                if (Fields.Count() > 0)
                {
                    foreach (var field in Fields)
                    {
                        FAttributes = field.GetCustomAttributes();
                        foreach (var Attr in FAttributes)
                        {
                            if (Attr != null)
                            {
                                if (Attr.GetType().FullName.IndexOf("BottomhalfCore.Annotations") != -1)
                                {
                                    if (ObjFieldAttributesCollection.Where(x => x.DeclaredName == field.Name).FirstOrDefault() == null)
                                    {
                                        ObjFieldAttributes = new BottomhalfModel.FieldAttributes();
                                        ObjFieldAttributes.Attributes.Add(Attr.GetType().Name);
                                        ObjFieldAttributes.FieldName = field.FieldType.FullName;
                                        ObjFieldAttributes.Name = field.FieldType.Name;
                                        ObjFieldAttributes.DeclaredName = field.Name;
                                        ObjFieldAttributes.IsGenericType = field.FieldType.IsGenericType;
                                        ObjFieldAttributes.IsField = true;
                                        if (field.FieldType.IsInterface)
                                            ObjFieldAttributes.IsInterface = true;
                                        else
                                            ObjFieldAttributes.IsInterface = false;
                                        ObjFieldAttributesCollection.Add(ObjFieldAttributes);
                                    }
                                }
                            }
                        }
                    }
                }

                var Properties = CurrentType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                if (Properties.Count() > 0)
                {
                    foreach (var property in Properties)
                    {
                        FAttributes = property.GetCustomAttributes();
                        foreach (var Attr in FAttributes)
                        {
                            if (Attr != null)
                            {
                                if (Attr.GetType().FullName.IndexOf("BottomhalfCore.Annotations") != -1)
                                {
                                    if (ObjFieldAttributesCollection.Where(x => x.DeclaredName == property.Name).FirstOrDefault() == null)
                                    {
                                        ObjFieldAttributes = new BottomhalfModel.FieldAttributes();
                                        ObjFieldAttributes.Attributes.Add(Attr.GetType().Name);
                                        ObjFieldAttributes.FieldName = property.PropertyType.FullName;
                                        ObjFieldAttributes.Name = property.PropertyType.Name;
                                        ObjFieldAttributes.DeclaredName = property.Name;
                                        ObjFieldAttributes.IsGenericType = property.PropertyType.IsGenericType;
                                        ObjFieldAttributes.IsField = false;
                                        if (property.PropertyType.IsInterface)
                                            ObjFieldAttributes.IsInterface = true;
                                        else
                                            ObjFieldAttributes.IsInterface = false;
                                        ObjFieldAttributesCollection.Add(ObjFieldAttributes);
                                    }
                                }
                            }
                        }
                    }
                }
                return ObjFieldAttributesCollection;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "InspectAttributes()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "InspectAttributes()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        #region CURRENT INTERFACE MARKUP DECLARATION

        public ClassAnnotationDetail InspectAnnotations(Type CurrentType, TypeRefCollection refCollection)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ParameterDetail> InspectMethods(Type CurrentType)
        {
            throw new NotImplementedException();
        }
     
        #endregion
    }
}

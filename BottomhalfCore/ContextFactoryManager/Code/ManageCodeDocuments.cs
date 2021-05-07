using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using BottomhalfCore.ContextFactoryManager.Interface;

namespace BottomhalfCore.ContextFactoryManager.Code
{
    public class ManageCodeDocuments : IManageCodeDocuments<ManageCodeDocuments>
    {
        /// <summary>GenerateDocumentation
        /// <para></para>
        /// </summary>
        public DocCollector GenerateDocumentation(Type CurrentType)
        {
            DocCollector ObjDocCollector = null;
            MethodDefination ObjMethodDefination = null;
            List<MethodDefination> ObjMethodDefinationlst = new List<MethodDefination>();
            List<string> TypeCollections = null;
            try
            {
                TypeCollections = new List<string>();
                foreach (var method in ((System.Reflection.TypeInfo)CurrentType).DeclaredMethods)
                {
                    IEnumerable<Attribute> Attributes = method.GetCustomAttributes();
                    foreach (Attribute attr in Attributes)
                    {
                        if (attr.GetType().Name == "Doc")
                        {
                            ObjMethodDefination = new MethodDefination();
                            ObjMethodDefination.MethodName = method.Name;
                            ObjMethodDefination.Summary = ((BottomhalfCore.Annotations.Doc)attr).Summary;
                            ObjMethodDefinationlst.Add(ObjMethodDefination);
                        }
                    }
                }

                if (ObjMethodDefinationlst.Count() > 0)
                {
                    ObjDocCollector = new DocCollector();
                    ObjDocCollector.ClassName = CurrentType.FullName;
                    ObjDocCollector.ObjMethodDefination = ObjMethodDefinationlst;
                }
                return ObjDocCollector;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "GenerateDocumentation()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "GenerateDocumentation()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }
    }
}

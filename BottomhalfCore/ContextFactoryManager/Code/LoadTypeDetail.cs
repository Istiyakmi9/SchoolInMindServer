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
    public class LoadTypeDetail : ILoasTypeDetail<LoadTypeDetail>
    {
        private List<Assembly> assemblyList = null;
        private readonly IManageCodeDocuments<ManageCodeDocuments> manageCodeDocuments;
        private readonly ITypeLoadingHelper<TypeLoadingHelper> typeLoadingHelper;

        public LoadTypeDetail(List<Assembly> assemblyList)
        {
            this.manageCodeDocuments = new ManageCodeDocuments();
            this.typeLoadingHelper = new TypeLoadingHelper();
            this.assemblyList = assemblyList;
        }

        /// <summary>BuildClassInformation
        /// <para></para>
        /// </summary>
        public Dictionary<string, TypeRefCollection> BuildClassInformation(List<AopDetail> aopDetailLst)
        {
            Assembly asm = null;
            int AssemblyIndex = 0;
            Dictionary<string, TypeRefCollection> ClassTypeCollection = new Dictionary<string, TypeRefCollection>();
            try
            {
                while (AssemblyIndex < assemblyList.Count)
                {
                    asm = assemblyList[AssemblyIndex];
                    // this.container.WriteToFile(asm.FullName);
                    Type[] TypeCollection = asm.GetTypes().Where(x => !x.IsEnum && !x.IsInterface && !x.IsAbstract).ToArray<Type>();
                    if (TypeCollection.Length > 0)
                    {
                        //this.container.WriteToFile(TypeCollection.Select(x => x.Name).ToList<string>());
                        var CollectionTypes = GetClassTypeCollection(TypeCollection, aopDetailLst);
                        foreach (var item in CollectionTypes)
                        {
                            if (!ClassTypeCollection.ContainsKey(item.Key))
                            {
                                ClassTypeCollection.Add(item.Key, item.Value);
                            }
                        }
                    }
                    AssemblyIndex++;
                }

                //Type Weaving = typeof(WeavProxy<>);
                //SpringClassInfo.Add(Weaving);
                //var SpringCollectionTypes = GetClassTypeCollection(SpringClassInfo.ToArray<Type>());
                //foreach (var item in SpringCollectionTypes)
                //{
                //    if (!ClassTypeCollection.ContainsKey(item.Key))
                //        ClassTypeCollection.Add(item.Key, item.Value);
                //}

                AddWeaving();


                return ClassTypeCollection;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "BuildClassInformation()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "BuildClassInformation()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>AddWeaving. Add weaving feature in this section
        /// </summary>
        private void AddWeaving()
        {

        }

        /// <summary>GetClassTypeCollection
        /// <para></para>
        /// </summary>
        private Dictionary<string, TypeRefCollection> GetClassTypeCollection(Type[] ActiveTypeCollection, List<AopDetail> aopDetailLst)
        {
            List<DocCollector> docCollectorlst = null;
            string KeyName = string.Empty;
            TypeRefCollection typeRefCollection = null;
            Dictionary<string, TypeRefCollection> ClassTypeCollection = new Dictionary<string, TypeRefCollection>();
            try
            {
                foreach (Type type in ActiveTypeCollection.ToList<Type>())
                {
                    if (type.Namespace != null && type.FullName.IndexOf("<") == -1 && type.FullName.IndexOf("<>") == -1 && !type.IsInterface && !type.IsAbstract)
                    {
                        typeRefCollection = new TypeRefCollection();
                        //FileName = ClassName.Replace(".cs", "").Trim();
                        //type = assemblyList.Where(x => x.FullName.Split(',')[0].ToLower() == asmName.ToLower())
                        //                   .SelectMany(a => a.GetTypes())
                        //                   .Where(i => i.Name.ToLower() == FileName.ToLower())
                        //                   .FirstOrDefault();

                        if (type != null)
                        {

                            /*---------------------- Reading and storing constructor detail of each class type. ---------------------------------
                            /                                                                                                                    /
                            / In this section it will iterate each classtype. Based on classtype this section will find all the constructor and  / 
                            / their parameter and also their detail. It will be used at the time of object creation and constructor selection.   /
                            /                                                                                                                    /
                            /                                                                                                                   */

                            DocCollector Docs = this.manageCodeDocuments.GenerateDocumentation(type);
                            if (Docs != null)
                            {
                                if (docCollectorlst == null)
                                    docCollectorlst = new List<DocCollector>();
                                docCollectorlst.Add(Docs);
                            }


                            /*-------------------------------------------- End Constructor reading --------------------------------------------- */



                            /*---------------------- Reading and storing constructor detail of each class type. ---------------------------------
                            /                                                                                                                    /
                            / In this section it will iterate each classtype. Based on classtype this section will find all the constructor and  / 
                            / their parameter and also their detail. It will be used at the time of object creation and constructor selection.   /
                            /                                                                                                                    /
                            /                                                                                                                   */

                            IFindClassAssets<FindConstructors> findClassAssets = new FindConstructors();
                            var CtorParam = findClassAssets.InspectMethods(type);
                            if (CtorParam != null && CtorParam.Count > 0)
                                typeRefCollection.Constructors = CtorParam;

                            /*-------------------------------------------- End Constructor reading --------------------------------------------- */



                            /*------------------------------ Reading and storing Class level annotation -----------------------------------------/
                            /                                                                                                                    /
                            / In this section it will iterate each classtype. Based on classtype this section will find all the constructor and  / 
                            / their parameter and also their detail. It will be used at the time of object creation and constructor selection.   /
                            /                                                                                                                    /
                            /                                                                                                                    /
                            / ------------------------------------------------------------------------------------------------------------------ */

                            IFindClassAssets<FindAnnotations> findAnnotations = new FindAnnotations();
                            var ClassAnnotations = findAnnotations.InspectAnnotations(type, typeRefCollection);
                            if (ClassAnnotations != null)
                            {
                                if (ClassAnnotations.annotationDefination != null && ClassAnnotations.annotationDefination.Count > 0)
                                    typeRefCollection.AnnotationNames = ClassAnnotations.annotationDefination;
                                aopDetailLst = ClassAnnotations.aopDetailLst;
                            }

                            /*-------------------------------------------- End Constructor reading --------------------------------------------- */






                            /*---------------------- Reading and storing attributes detail of each class type. ---------------------------------
                            /                                                                                                                    /
                            / In this section it will iterate each classtype. Based on classtype this section will find all the constructor and  / 
                            / their parameter and also their detail. It will be used at the time of object creation and constructor selection.   /
                            /                                                                                                                   */

                            IFindClassAssets<FindAttributes> findAttributes = new FindAttributes();
                            var TypeAttr = findAttributes.InspectAttributes(type);
                            if (TypeAttr != null && TypeAttr.Count > 0)
                                typeRefCollection.FieldAttributesCollection = TypeAttr;

                            /*                                                                                                                   /
                            /                                                                                                                    /
                            / ------------------------------------------------------------------------------------------------------------------ */

                            var AllBases = this.typeLoadingHelper.GetDeepBaseTypes(type, this.assemblyList);
                            if (AllBases != null)
                                typeRefCollection.BaseTypeHirarchy = AllBases;
                            //typeRefCollection.ClassType = type;
                            typeRefCollection.AssemblyQualifiedName = type.AssemblyQualifiedName;
                            typeRefCollection.IsContainsGenericParameters = type.ContainsGenericParameters;
                            typeRefCollection.IsInterface = type.IsInterface;
                            typeRefCollection.IsClass = type.IsClass;
                            typeRefCollection.ClassName = type.Name;
                            typeRefCollection.ClassFullyQualifiedName = type.FullName;
                            typeRefCollection.AssemblyName = type.Assembly.GetName().Name;
                            typeRefCollection.IsFullyCreated = true;
                            if (type.Name.IndexOf("`") != -1)
                                KeyName = type.Name.Split('`')[0];
                            else
                                KeyName = type.Name;
                            ClassTypeCollection.Add(type.FullName.ToLower(), typeRefCollection);
                        }
                    }
                }

                return ClassTypeCollection;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "GetClassTypeCollection()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "GetClassTypeCollection()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }
    }
}

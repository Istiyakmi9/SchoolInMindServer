using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.ContextFactoryManager.Interface;
using BottomhalfCore.Exceptions;
using BottomhalfCore.FactoryContext;
using BottomhalfCore.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Code
{
    public class ManageTypeCollection : IManageTypeCollection<ManageTypeCollection>
    {
        private IContainer container;
        public ManageTypeCollection()
        {
            this.container = Container.GetInstance();
        }


        /// <summary>GetDbRefType
        /// <para></para>
        /// </summary>
        public TypeRefCollection GetDbRefType()
        {
            try
            {
                BottomhalfCore.DatabaseLayer.MsSql.Code.Db db = new BottomhalfCore.DatabaseLayer.MsSql.Code.Db(null);
                Type DbType = db.GetType();
                TypeRefCollection typeRefCollection = new TypeRefCollection();
                typeRefCollection.ClassName = DbType.Name;
                typeRefCollection.ClassFullyQualifiedName = DbType.FullName;
                //typeRefCollection.ClassType = DbType;
                typeRefCollection.IsClass = DbType.IsClass;
                typeRefCollection.IsInterface = DbType.IsInterface;
                typeRefCollection.AssemblyQualifiedName = DbType.AssemblyQualifiedName;
                typeRefCollection.IsFullyCreated = true;
                typeRefCollection.AssemblyName = this.GetType().Assembly.FullName;
                return typeRefCollection;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "GetDbRefType()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "GetDbRefType()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        public TypeRefCollection GetMySqlDbRefType()
        {
            try
            {
                BottomhalfCore.DatabaseLayer.MySql.Code.Db db = new BottomhalfCore.DatabaseLayer.MySql.Code.Db(null);
                Type DbType = db.GetType();
                TypeRefCollection typeRefCollection = new TypeRefCollection();
                typeRefCollection.ClassName = DbType.Name;
                typeRefCollection.ClassFullyQualifiedName = DbType.FullName;
                typeRefCollection.IsFullyCreated = true;
                typeRefCollection.AssemblyQualifiedName = DbType.AssemblyQualifiedName;
                typeRefCollection.AssemblyName = this.GetType().Assembly.FullName;
                return typeRefCollection;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "GetDbRefType()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "GetDbRefType()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>MapInterfaceToClasses
        /// <para></para>
        /// </summary>
        public Dictionary<string, List<InterfaceClassLinker>> MapInterfaceToClasses(Dictionary<string, TypeRefCollection> TypeCollection, List<AopDetail> aopDetailLst)
        {
            IManageAopDetail<ManageAopDetail> manageAopDetail;
            Dictionary<string, List<InterfaceClassLinker>> InterfaceClassMapper = new Dictionary<string, List<InterfaceClassLinker>>();
            List<InterfaceClassLinker> interfaceClassLinkerList = null;
            InterfaceClassLinker interfaceClassLinker = null;
            try
            {
                if (TypeCollection != null && TypeCollection.Count > 0)
                {
                    foreach (var TypeRef in TypeCollection)
                    {
                        manageAopDetail = new ManageAopDetail();
                        manageAopDetail.FindAopOnClass(TypeRef.Value, aopDetailLst);
                        interfaceClassLinkerList = new List<InterfaceClassLinker>();
                        foreach (var Base in TypeRef.Value.BaseTypeHirarchy)
                        {
                            interfaceClassLinker = new InterfaceClassLinker();
                            interfaceClassLinker.FullName = TypeRef.Value.ClassFullyQualifiedName;
                            interfaceClassLinker.Name = TypeRef.Value.ClassName;

                            var ExistingObject = InterfaceClassMapper.Where(x => x.Key == Base.TypeFullName).FirstOrDefault();
                            if (ExistingObject.Key == null || ExistingObject.Value == null)
                                ExistingObject = InterfaceClassMapper.Where(x => x.Key == Base.TypeName).FirstOrDefault();
                            if (ExistingObject.Key != null && ExistingObject.Value != null)
                            {
                                interfaceClassLinkerList = ExistingObject.Value;

                                var ObjReferenced = interfaceClassLinkerList.Where(x => x.FullName == interfaceClassLinker.FullName ||
                                                    x.Name == interfaceClassLinker.Name).FirstOrDefault();
                                if (ObjReferenced == null)
                                {
                                    interfaceClassLinkerList.Add(interfaceClassLinker);
                                    if (Base.TypeFullName != null)
                                        InterfaceClassMapper[Base.TypeFullName] = interfaceClassLinkerList;
                                    else
                                        InterfaceClassMapper[Base.TypeName] = interfaceClassLinkerList;
                                }
                            }
                            else
                            {
                                interfaceClassLinkerList.Add(interfaceClassLinker);
                                if (Base.TypeFullName != null)
                                    InterfaceClassMapper.Add(Base.TypeFullName, interfaceClassLinkerList);
                                else
                                    InterfaceClassMapper.Add(Base.TypeName, interfaceClassLinkerList);
                            }
                        }
                    }
                }
                return InterfaceClassMapper;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "MapInterfaceToClasses()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "MapInterfaceToClasses()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>ManageConstructor
        /// <para></para>
        /// </summary>
        public void ManageConstructor(Dictionary<string, TypeRefCollection> ClassTypeCollection)
        {
            try
            {
                string Element = null;
                ParameterNameCollection ParamCollectionName = null;
                int typeindex = 0, CtorIndex = 0, paramindex = 0;
                List<string> Properties = null;
                TypeRefCollection TypeRef = null;
                ParameterDetail ObjParameterDetail = null;
                Boolean RemoveFlag = false;
                Dictionary<string, List<InterfaceClassLinker>> interfaceClassLinker = container.GetInterfaceClassLinker();
                INameSpaceHandler nameSpaceHandler = new NameSpaceHandler();
                while (typeindex < ClassTypeCollection.Count)
                {
                    TypeRef = ClassTypeCollection.ElementAt(typeindex).Value;

                    //--------------------------------- Managing attribute  ---------------------------------------------------
                    if (TypeRef.FieldAttributesCollection != null && TypeRef.FieldAttributesCollection.Count > 0)
                    {
                        int fieldindex = 0;
                        Properties = new List<string>();
                        while (fieldindex < TypeRef.FieldAttributesCollection.Count)
                        {
                            var FieldName = TypeRef.FieldAttributesCollection[fieldindex].FieldName;
                            if (FieldName != null && FieldName.IndexOf('`') != -1)
                            {
                                Element = null;
                                Element = FieldName.Substring(0, FieldName.IndexOf("[["));
                                var NewObj = interfaceClassLinker.Where(x => x.Key == Element).FirstOrDefault().Value;
                                if (NewObj != null && NewObj.Count > 0)
                                {
                                    if (TypeRef.FieldAttributesCollection[fieldindex].IsInterface)
                                    {
                                        List<string> SplittedCollection = ConvertNameSpaceToList(FieldName);

                                        /// <summary>
                                        /// Handle and manage class or types namespace
                                        /// </summary>
                                        SplittedCollection = nameSpaceHandler.ResolveNamespace(SplittedCollection);
                                        if (SplittedCollection.Count == 2)
                                        {
                                            int removeindex = 0;
                                            while (removeindex < SplittedCollection.Count)
                                            {
                                                if (SplittedCollection[removeindex].IndexOf("`") != 1)
                                                    SplittedCollection.RemoveAt(removeindex);
                                                removeindex++;
                                            }
                                        }
                                        else
                                        {
                                            SplittedCollection[0] = NewObj.FirstOrDefault().FullName; //ParamCollectionName.Name;
                                        }

                                        if (SplittedCollection.Count == 1)
                                        {
                                            TypeRef.FieldAttributesCollection[fieldindex].FieldName = SplittedCollection[0];
                                            TypeRef.FieldAttributesCollection[fieldindex].GenericList = null;
                                            Properties.Add(SplittedCollection[0]);
                                        }
                                        else
                                        {
                                            TypeRef.FieldAttributesCollection[fieldindex].FieldName = null;
                                            TypeRef.FieldAttributesCollection[fieldindex].GenericList = SplittedCollection;
                                            Properties.AddRange(SplittedCollection);
                                        }
                                    }
                                }
                            }
                            fieldindex++;
                        }
                    }

                    //-------------------------------------------- Managing controller -------------------------------------------
                    if (TypeRef != null && TypeRef.Constructors.Count > 0)
                    {
                        CtorIndex = 0;
                        while (CtorIndex < TypeRef.Constructors.Count)
                        {
                            ObjParameterDetail = TypeRef.Constructors.ElementAt(CtorIndex).Value;
                            if (ObjParameterDetail != null && ObjParameterDetail.Parameters.Count > 0)
                            {
                                paramindex = 0;
                                while (paramindex < ObjParameterDetail.Parameters.Count)
                                {
                                    RemoveFlag = false;
                                    ParamCollectionName = ObjParameterDetail.Parameters.ElementAt(paramindex);
                                    //------------->  Converting generic collection types -----------------------------
                                    if (ParamCollectionName.Name.IndexOf("`") != -1)
                                    {
                                        if ((ParamCollectionName.Name.Split(new char[] { '`' })[0]).IndexOf("System.Collections") == -1)
                                        {
                                            if (ParamCollectionName.Name.IndexOf("[[") != -1)
                                                Element = ParamCollectionName.Name.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                            else
                                                Element = ParamCollectionName.Name;
                                            if (Element.IndexOf("`1") != -1)
                                            {
                                                if (ParamCollectionName.Name.IndexOf("[[") != -1)
                                                    Element = ParamCollectionName.Name.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries)[1];
                                                else
                                                    Element = ParamCollectionName.Name;
                                                ParamCollectionName.Name = Element.Split(new[] { ',' })[0];
                                                RemoveFlag = true;
                                            }
                                            else
                                            {
                                                var NewObj = interfaceClassLinker.Where(x => x.Key == Element).FirstOrDefault().Value;
                                                if (NewObj != null)
                                                {
                                                    RemoveFlag = true;
                                                    ParamCollectionName.Name = NewObj.FirstOrDefault().FullName;
                                                }
                                            }
                                            ObjParameterDetail.Parameters[paramindex++] = ParamCollectionName;
                                        }
                                        else
                                        {
                                            RemoveFlag = true;
                                            Element = GetImplementedName(ParamCollectionName.Name);
                                            ObjParameterDetail.Parameters[paramindex++] = ParamCollectionName;
                                        }
                                    }
                                    if (!RemoveFlag)
                                        paramindex++;
                                }
                            }
                            CtorIndex++;
                        }
                    }
                    typeindex++;
                }
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "ManageConstructor()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "ManageConstructor()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>GetImplementedName
        /// <para></para>
        /// </summary>
        public string GetImplementedName(string GenericName)
        {
            string ModifiedName = null;
            string Element = null;
            Element = GenericName.Split(new char[] { '`' })[0];
            if (Element != null)
            {
                if (Element.IndexOf("IDictionary") != -1)
                {
                    Element = Element.Replace("IDictionary", "Dictionary");
                }
                else if (Element.IndexOf("IList") != -1)
                {
                    Element = Element.Replace("IList", "List");
                }
                else if (Element.IndexOf("IEnumerable") != -1)
                {
                    Element = Element.Replace("IEnumerable", "Enumerable");
                }
            }
            ModifiedName = Element + GenericName.Substring(GenericName.IndexOf("`"), GenericName.Length - GenericName.IndexOf("`"));
            return ModifiedName;
        }

        /// <summary>ConvertNameSpaceToList
        /// <para></para>
        /// </summary>
        public List<string> ConvertNameSpaceToList(string SplittedName)
        {
            List<string> NameCollection = new List<string>();
            var CollectionData = SplittedName.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries);
            if (CollectionData.ToList<string>().Count > 0)
            {
                if (CollectionData[0].IndexOf("System.Collections") == -1)
                {
                    if (SplittedName.IndexOf("System.Collections") != -1)
                    {
                        var Data = SplittedName.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in Data)
                        {
                            if (item.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries)[0].IndexOf("System.Collections") != -1)
                            {
                                NameCollection.Add(item);
                            }
                            else
                            {
                                if (item.IndexOf("[[") != -1)
                                {
                                    var FirstPart = item.Substring(0, item.IndexOf("[["));
                                    NameCollection.Add(FirstPart);
                                    string LastPart = item.Substring(item.IndexOf("[[") + 2, item.Length - item.IndexOf("[[") - 2);
                                    var PartedCollection = ConvertNameSpaceToList(LastPart);
                                    if (PartedCollection.Count > 0)
                                        NameCollection.AddRange(PartedCollection);
                                }
                                else
                                {
                                    var LastItem = item.Split(new[] { ',' });
                                    if (LastItem.Count() > 0)
                                        NameCollection.Add(LastItem[0]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CollectionData.Count() > 0)
                            NameCollection.AddRange(CollectionData);
                        return NameCollection;
                    }
                }
                else
                {
                    NameCollection.Add(SplittedName);
                    return NameCollection;
                }
            }

            return NameCollection;
        }
    }
}

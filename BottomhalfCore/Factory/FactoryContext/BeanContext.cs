using Bottomhalf.SpringNetModel;
using BottomhalfCore.Annotations;
using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.CacheManagement.Caching;
using BottomhalfCore.CacheManagement.CachingInterface;
using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.DiService;
using BottomhalfCore.Exceptions;
using BottomhalfCore.Flags;
using BottomhalfCore.IFactoryContext;
using BottomhalfCore.Model;
using BottomhalfCore.Services.Code;
using BottomhalfCore.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace BottomhalfCore.FactoryContext
{
    public class BeanContext
    {
        public IContainer container;
        public readonly ICacheManager<CacheManager> cacheManager;
        private Type type;
        public Type ParameterType = null;
        private static BeanContext context;
        private static readonly Object _lock = new object();
        private TypeRefCollection TypeWithRefList = null;
        private ApplicationDetail applicationDetail = null;

        private BeanContext()
        {
            try
            {
                container = Container.GetInstance();
                cacheManager = CacheManager.GetInstance();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitBottomhalf()
        {
            try
            {
                ResolveDependencies();
                CreatBeanInstances();
                this.cacheManager.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResolveDependencies()
        {
            try
            {
                Boolean ContainerState = container.IsReadyContainerInvoked();
                ResolverClassType ObjResolverClassType = null;
                if (!ContainerState)
                    ObjResolverClassType = new ResolverClassType();
            }
            catch (BeanException beanEx)
            {
                if (container == null)
                    container = Container.GetInstance();
                container.LogError(beanEx, beanEx.GetFullMessage());
                throw beanEx;
            }
            catch (Exception ex)
            {
                if (container == null)
                    container = Container.GetInstance();
                container.LogError(ex, "");
                BeanException beanException = new BeanException();
                beanException.SetMessage(ex.Message);
                beanException.SetExceptionPath("Error path: ResolveDependencies().");
                throw beanException;
            }
        }

        public static BeanContext GetInstance()
        {
            try
            {
                if (context == null)
                {
                    lock (_lock)
                    {
                        if (context == null)
                        {
                            context = new BeanContext();
                        }
                    }
                }
                return context;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ADD CONFIGURATION SETTING METHODS

        public BeanContext AddDatabase(string ConnectionString, SqlType sqlType)
        {
            this.container.SetConnectionString(ConnectionString);
            this.container.SetSQLType(sqlType);
            return this;
        }

        //public BeanContext AddClassLibraries(List<string> Libraries)
        //{
        //    this.container.AddProjectName(Libraries);
        //    return this;
        //}

        public List<Type> GetAccessorTypes()
        {
            IDictionary<string, GraphContainerModal> GraphContainerModal = this.container.GetGraphContainerModalCollection();
            List<Type> types = new List<Type>();
            int Count = GraphContainerModal.Where(x => x.Key.ToLower().Contains("accessor")).Select(x => x).Count();
            if (Count > 0)
            {
                var AccessorTypes = GraphContainerModal.Where(x => x.Key.ToLower().Contains("accessor")).Select(x => x);
                //foreach (var Accessors in AccessorTypes)
                //{
                //    GraphContainerModal InnerType = Accessors.Value;
                //    var AllInnerTypes = InnerType.TypeDetail.Select(x => x.Value.AssemblyQualifiedName).ToList<Type>();
                //    foreach (Type SingleType in AllInnerTypes)
                //    {
                //        if (!types.Contains(SingleType))
                //            types.Add(SingleType);
                //    }
                //}
            }

            return types;
        }

        public BeanContext Load()
        {
            this.InitBottomhalf();
            return this;
        }

        #endregion

        public static void DestroyContext()
        {
            context = null;
        }

        public void CreatBeanInstances()
        {
            try
            {
                #region Annotation Call

                container = Container.GetInstance();
                ReadAnnotation();

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FindBeanId(Type RequestedClassType)
        {
            string beanId = null;
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(RequestedClassType);
            foreach (Attribute attr in attrs)
            {
                if (IsBottomhalfComponent(attr.GetType()))
                {
                    var BeanType = attr as Component;
                    if (BeanType.Id != null && BeanType.Id != "")
                        beanId = BeanType.Id;
                    else
                        beanId = RequestedClassType.Name.ToLower();
                    return beanId;
                }
            }
            return beanId;
        }

        public void ReadAnnotation()
        {
            IDictionary<string, GraphContainerModal> BeanModal = null;
            container = Container.GetInstance();
            ConcurrentDictionary<string, TypeRefCollection> BeanCollection = null;
            BeanModal = container.GetGraphContainerModalCollection();
            TypeRefCollection Bean = null;
            int index = 0;
            while (index < BeanModal.Count())
            {
                KeyValuePair<string, GraphContainerModal> _graphContainerModal = BeanModal.ElementAt(index);
                BeanCollection = _graphContainerModal.Value.TypeDetail;
                for (var i = 0; i < BeanCollection.Count(); i++)
                {
                    KeyValuePair<string, TypeRefCollection> objectType = BeanCollection.ElementAt(i);
                    type = Type.GetType(objectType.Value.AssemblyQualifiedName);
                    if (type != null && type.IsInterface)
                    {
                        Bean = null;
                        BeanCollection.TryRemove(BeanCollection.ElementAt(i).Key, out Bean);
                        continue;
                    }

                    if (type != null && !type.IsAbstract && !type.IsInterface)
                    {
                        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(type);
                        if (attrs.Count() > 0)
                        {
                            if (type.BaseType.Name == "Controller" || type.BaseType.Name == "ApiController")
                            {
                                StereoTypeBeans(type, null);
                            }
                            else
                            {
                                foreach (Attribute attr in attrs)
                                {
                                    string annotationName = null;
                                    if (IsBottomhalfComponent(attr.GetType()))
                                    {
                                        string value = (attr as Component).Id;
                                        switch (attr.GetType().Name)
                                        {
                                            case "Autowired":
                                                annotationName = attr.ToString();
                                                break;
                                            case "Component":
                                                StereoTypeBeans(type, attr);
                                                break;
                                            case "Lazy":
                                                annotationName = attr.ToString();
                                                break;
                                            case "Value":
                                                annotationName = attr.ToString();
                                                break;
                                            case "Required":
                                                annotationName = attr.ToString();
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            StereoTypeBeans(type, null);
                        }
                    }
                }
                index++;
            }
        }

        public bool IsBottomhalfComponent(Type AttrType)
        {
            bool IsAvailable = false;
            IList<string> AnnotationName = new List<string>() {
                "Component", "Autowired", "Bean", "RestController", "Configuration", "Repository", "Required", "Value", "Service" };
            foreach (string annotation in AnnotationName)
            {
                if (AttrType.Name.ToLower() == annotation.ToLower())
                {
                    IsAvailable = true;
                    return IsAvailable;
                }
            }
            return false;
        }

        public string ComputeSha256Hash(string Key)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Key));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void StereoTypeBeans(Type ClassType, Attribute BeanAttribute)
        {
            string beanId = null;
            TypeRefCollection TypeWithRefList = null;
            if (BeanAttribute != null)
            {
                var BeanType = BeanAttribute as Component;
                if (BeanType.Id != null && BeanType.Id != "")
                    beanId = BeanType.Id;
                else
                    beanId = type.FullName;
            }

            if (beanId != null && beanId != "")
            {
                TypeWithRefList = null;
                var objClassTypeList = new Dictionary<string, Dictionary<Type, List<string>>>();
                objClassTypeList.Add(ClassType.FullName, null);
                TypeWithRefList = container.GetTypeRefByName(ClassType.FullName);
                if (TypeWithRefList != null)
                {
                    if (TypeWithRefList.IsFullyCreated)
                        TypeWithRefList.AliasName = beanId;
                    else
                    {
                        try
                        {
                            if (!TypeWithRefList.IsFullyCreated)
                            {
                                BeanException ObjBeanException = new BeanException();
                                ObjBeanException.SetMessage("Not able to create or find all the dependencies. Container creation error.");
                                string Message = "";
                                if (TypeWithRefList.ClassRefNames.Count() > 0)
                                {
                                    Message += "Dependency list names: ";
                                    Message += String.Join(",", TypeWithRefList.ClassRefNames);
                                    Message += "\n";
                                }

                                Message += "Fully qualified path for class: " + TypeWithRefList.ClassFullyQualifiedName;
                                ObjBeanException.SetExceptionPath(Message);
                                throw ObjBeanException;
                            }
                        }
                        catch (CircularReference ex)
                        {
                            throw new ApplicationException(ex.Message);
                        }
                    }
                }
            }
        }

        public bool IsControllerBaseClass(Type CurrentClassType)
        {
            bool isController = false;
            while (CurrentClassType != null)
            {
                if (CurrentClassType.Name == "ApiController" || CurrentClassType.Name == "Controller")
                {
                    isController = true;
                    break;
                }
                if (CurrentClassType.Name.ToLower() == "object")
                {
                    isController = false;
                    break;
                }
                CurrentClassType = CurrentClassType.BaseType;
            }
            return isController;
        }

        public void LogException(Exception exception)
        {
            string FilePath = @"E:\Projects\OnlineDataBuilderServer\ErrorLogs";
            //string FilePath = @"C:\inetpub\wwwroot\ErrorLogs";
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            else
            {
                string FileName = "E-Databuilder-" + (DateTime.Now.ToShortDateString().Replace("/", "-") + "__" + DateTime.Now.ToShortTimeString().Replace(":", "-")).Replace(" ", "") + ".txt";
                FilePath = System.IO.Path.Combine(FilePath, FileName);
                if (!System.IO.File.Exists(FilePath))
                {
                    StringBuilder buffer = new StringBuilder();
                    buffer.Append("Message: " + exception.Message);
                    buffer.Append("InnerMessage: " + exception.InnerException);
                    buffer.Append("StackTrace: " + exception.StackTrace);
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        Byte[] title = new UTF8Encoding(true).GetBytes(buffer.ToString());
                        fs.WriteAsync(title, 0, title.Length);
                    }
                }
            }
        }

        #region CREATE BEAN OBJECT

        public Type GetBeanType(string ClassFullName)
        {
            TypeRefCollection TypeWithRefList = container.GetTypeRefByName(ClassFullName);
            if (TypeWithRefList != null)
                return Type.GetType(TypeWithRefList.AssemblyQualifiedName);
            return null;
        }

        /// <summary>
        /// <para />Get Object of type T of generic form
        /// <para />e.g. GetBean<T>() => Get Type T object
        /// </summary>
        public T GetBean<T>()
        {
            Type ClassType = typeof(T);
            TypeWithRefList = null;
            string ClassName = ClassType.FullName;
            Object NewObject = null;
            Object HelperObject = null;
            Dictionary<string, Object> CreatedObjects = null;
            List<BottomhalfModel.FieldAttributes> AnnotattedFields = null;
            BottomhalfModel.FieldAttributes fieldAttributes = null;
            try
            {
                if (container.IsReadyContainerInvoked())
                {
                    if (context == null)
                        context = BeanContext.GetInstance();

                    TypeWithRefList = container.GetTypeRefByName(ClassName);
                    if (TypeWithRefList != null && TypeWithRefList.IsAOPEnabled)
                        ClassName = "weavproxy";
                    CreatedObjects = new Dictionary<string, object>();
                    NewObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, null);
                    if (NewObject == null)
                    {
                        SessionException exception = new SessionException();
                        exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                        exception.ExceptionCode = EFlags.ClassNotFount;
                        throw exception;
                    }
                    else
                    {
                        if (ClassName == "weavproxy")
                        {
                            MethodInfo Method = NewObject.GetType().GetMethods().Where(x => x.Name == "GetTransparentProxy").FirstOrDefault();
                            NewObject = Method.Invoke(NewObject, null);
                        }
                    }

                    TypeWithRefList = null;
                    TypeWithRefList = container.GetTypeRefByName(ClassName);
                    if (TypeWithRefList != null)
                    {
                        ParameterType = null;
                        ClassName = null;
                        ParameterType = Type.GetType(TypeWithRefList.AssemblyQualifiedName);
                        AnnotattedFields = TypeWithRefList.FieldAttributesCollection;
                        if (AnnotattedFields != null)
                        {
                            int fieldIndex = 0;
                            while (fieldIndex < AnnotattedFields.Count())
                            {
                                fieldAttributes = AnnotattedFields[fieldIndex];
                                if (CreatedObjects.Where(x => x.Key == fieldAttributes.FieldName).FirstOrDefault().Key == null)
                                {
                                    if (fieldAttributes != null && !string.IsNullOrEmpty(fieldAttributes.DeclaredName) && fieldAttributes.IsField)
                                    {
                                        HelperObject = null;
                                        int attrindex = 0;
                                        var Fields = ((System.Reflection.TypeInfo)ParameterType).DeclaredFields;
                                        while (attrindex < fieldAttributes.Attributes.Count)
                                        {
                                            if (fieldAttributes.Attributes[attrindex] == "Autowired")
                                            {
                                                var Field = Fields.Where(x => x.Name == fieldAttributes.DeclaredName).FirstOrDefault();
                                                if (Field != null)
                                                {
                                                    ClassName = fieldAttributes.Name;
                                                    CreatedObjects = new Dictionary<string, object>();
                                                    HelperObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, null);
                                                    if (HelperObject == null)
                                                    {
                                                        SessionException exception = new SessionException();
                                                        exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                                                        exception.ExceptionCode = EFlags.ClassNotFount;
                                                        throw exception;
                                                    }
                                                    else
                                                    {
                                                        if (HelperObject != null)
                                                            Field.SetValue(NewObject, HelperObject);
                                                    }
                                                }
                                            }
                                            attrindex++;
                                        }
                                    }
                                    else
                                    {
                                        HelperObject = null;
                                        int attrindex = 0;
                                        var Properties = ((System.Reflection.TypeInfo)ParameterType).DeclaredProperties;
                                        while (attrindex < fieldAttributes.Attributes.Count)
                                        {
                                            if (fieldAttributes.Attributes[attrindex] == "Autowired")
                                            {
                                                var Property = Properties.Where(x => x.Name == fieldAttributes.DeclaredName).FirstOrDefault();
                                                if (Property != null)
                                                {
                                                    ClassName = fieldAttributes.FieldName;
                                                    CreatedObjects = new Dictionary<string, object>();
                                                    HelperObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, null);
                                                    if (HelperObject == null)
                                                    {
                                                        SessionException exception = new SessionException();
                                                        exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                                                        exception.ExceptionCode = EFlags.ClassNotFount;
                                                        throw exception;
                                                    }
                                                    else
                                                    {
                                                        if (HelperObject != null)
                                                            Property.SetValue(NewObject, HelperObject);
                                                    }
                                                }
                                            }
                                            attrindex++;
                                        }
                                    }
                                }
                                fieldIndex++;
                            }
                        }
                    }
                }
                else
                {
                    SessionException exception = new SessionException();
                    exception.SetMessage(exception.GenerateMessage(EFlags.ClassNotFount));
                    exception.ExceptionCode = EFlags.ClassNotFount;
                    throw exception;
                }
                CreatedObjects = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (T)NewObject;
        }

        /// <summary>
        /// <para />Use this method whenever you want to create instance of a class
        /// <para />ClassType: pass ClassType. e.g. typeof(SomeClass)
        /// </summary>
        public Object GetBean(Type ClassType, HttpContext httpContext = null)
        {
            string ClassName = ClassType.FullName;
            string CallingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            TypeWithRefList = null;
            Object NewObject = null;
            Object HelperObject = null;
            Dictionary<string, Object> CreatedObjects = null;
            List<BottomhalfModel.FieldAttributes> AnnotattedFields = null;
            BottomhalfModel.FieldAttributes fieldAttributes = null;
            try
            {
                if (container.IsReadyContainerInvoked())
                {
                    if (context == null)
                        context = BeanContext.GetInstance();

                    TypeWithRefList = container.GetTypeRefByName(ClassName);
                    if (TypeWithRefList != null && TypeWithRefList.IsAOPEnabled)
                        ClassName = "weavproxy";
                    CreatedObjects = new Dictionary<string, object>();
                    NewObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, httpContext);
                    if (ClassName == "weavproxy")
                    {
                        MethodInfo Method = NewObject.GetType().GetMethods().Where(x => x.Name == "GetTransparentProxy").FirstOrDefault();
                        NewObject = Method.Invoke(NewObject, null);
                    }
                    if (NewObject == null)
                    {
                        SessionException exception = new SessionException();
                        exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                        exception.ExceptionCode = EFlags.ClassNotFount;
                        throw exception;
                    }

                    TypeWithRefList = null;
                    TypeWithRefList = container.GetTypeRefByName(ClassName);
                    if (TypeWithRefList != null)
                    {
                        ParameterType = null;
                        ClassName = null;
                        ParameterType = Type.GetType(TypeWithRefList.AssemblyQualifiedName);
                        AnnotattedFields = TypeWithRefList.FieldAttributesCollection;
                        if (AnnotattedFields != null)
                        {
                            int fieldIndex = 0;
                            while (fieldIndex < AnnotattedFields.Count())
                            {
                                fieldAttributes = AnnotattedFields[fieldIndex];
                                if (fieldAttributes != null && !string.IsNullOrEmpty(fieldAttributes.DeclaredName) && fieldAttributes.IsField)
                                {
                                    HelperObject = null;
                                    int attrindex = 0;
                                    var Fields = ((System.Reflection.TypeInfo)ParameterType).DeclaredFields;
                                    while (attrindex < fieldAttributes.Attributes.Count)
                                    {
                                        if (fieldAttributes.Attributes[attrindex] == "Autowired")
                                        {
                                            var CurrentType = Type.GetType(fieldAttributes.FieldName);
                                            var Field = Fields.Where(x => x.Name == fieldAttributes.DeclaredName).FirstOrDefault();
                                            if (Field != null)
                                            {
                                                ClassName = fieldAttributes.FieldName;
                                                CreatedObjects = new Dictionary<string, object>();
                                                HelperObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, httpContext);
                                                if (HelperObject == null)
                                                {
                                                    SessionException exception = new SessionException();
                                                    exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                                                    exception.ExceptionCode = EFlags.ClassNotFount;
                                                    throw exception;
                                                }
                                                else
                                                {
                                                    if (HelperObject != null)
                                                        Field.SetValue(NewObject, HelperObject);
                                                }
                                            }
                                        }
                                        attrindex++;
                                    }
                                }
                                else
                                {
                                    HelperObject = null;
                                    int attrindex = 0;
                                    var Properties = ((System.Reflection.TypeInfo)ParameterType).DeclaredProperties;
                                    while (attrindex < fieldAttributes.Attributes.Count)
                                    {
                                        if (fieldAttributes.Attributes[attrindex] == "Autowired")
                                        {
                                            var Property = Properties.Where(x => x.Name == fieldAttributes.DeclaredName).FirstOrDefault();
                                            if (Property != null)
                                            {
                                                ClassName = fieldAttributes.Name;
                                                CreatedObjects = new Dictionary<string, object>();
                                                HelperObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, httpContext);
                                                if (HelperObject == null)
                                                {
                                                    SessionException exception = new SessionException();
                                                    exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                                                    exception.ExceptionCode = EFlags.ClassNotFount;
                                                    throw exception;
                                                }
                                                else
                                                {
                                                    if (HelperObject != null)
                                                        Property.SetValue(NewObject, HelperObject);
                                                }
                                            }
                                        }
                                        attrindex++;
                                    }
                                }
                                fieldIndex++;
                            }
                        }
                    }
                }
                else
                {
                    SessionException exception = new SessionException();
                    exception.SetMessage(exception.GenerateMessage(EFlags.ClassNotFount));
                    exception.ExceptionCode = EFlags.ClassNotFount;
                    throw exception;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NewObject;
        }

        /// <summary>
        /// <para />Use this method whenever you want to create instance of a class
        /// <para />ClassType: pass ClassType. e.g. typeof(SomeClass)
        /// </summary>
        public Object GetBean(string ClassName, HttpContext httpContext = null)
        {
            string CallingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            TypeWithRefList = null;
            Object NewObject = null;
            Object HelperObject = null;
            Dictionary<string, Object> CreatedObjects = null;
            List<BottomhalfModel.FieldAttributes> AnnotattedFields = null;
            BottomhalfModel.FieldAttributes fieldAttributes = null;
            try
            {
                if (container.IsReadyContainerInvoked())
                {
                    if (context == null)
                        context = BeanContext.GetInstance();

                    TypeWithRefList = container.GetTypeRefByName(ClassName);
                    if (TypeWithRefList != null && TypeWithRefList.IsAOPEnabled)
                        ClassName = "weavproxy";
                    CreatedObjects = new Dictionary<string, object>();
                    NewObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, httpContext);
                    if (ClassName == "weavproxy")
                    {
                        MethodInfo Method = NewObject.GetType().GetMethods().Where(x => x.Name == "GetTransparentProxy").FirstOrDefault();
                        NewObject = Method.Invoke(NewObject, null);
                    }
                    if (NewObject == null)
                    {
                        SessionException exception = new SessionException();
                        exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                        exception.ExceptionCode = EFlags.ClassNotFount;
                        throw exception;
                    }

                    TypeWithRefList = null;
                    TypeWithRefList = container.GetTypeRefByName(ClassName);
                    if (TypeWithRefList != null)
                    {
                        ParameterType = null;
                        ClassName = null;
                        ParameterType = Type.GetType(TypeWithRefList.AssemblyQualifiedName);
                        AnnotattedFields = TypeWithRefList.FieldAttributesCollection;
                        if (AnnotattedFields != null)
                        {
                            int fieldIndex = 0;
                            while (fieldIndex < AnnotattedFields.Count())
                            {
                                fieldAttributes = AnnotattedFields[fieldIndex];
                                if (fieldAttributes != null && !string.IsNullOrEmpty(fieldAttributes.DeclaredName) && fieldAttributes.IsField)
                                {
                                    HelperObject = null;
                                    int attrindex = 0;
                                    var Fields = ((System.Reflection.TypeInfo)ParameterType).DeclaredFields;
                                    while (attrindex < fieldAttributes.Attributes.Count)
                                    {
                                        if (fieldAttributes.Attributes[attrindex] == "Autowired")
                                        {
                                            var CurrentType = Type.GetType(fieldAttributes.FieldName);
                                            var Field = Fields.Where(x => x.Name == fieldAttributes.DeclaredName).FirstOrDefault();
                                            if (Field != null)
                                            {
                                                ClassName = fieldAttributes.FieldName;
                                                CreatedObjects = new Dictionary<string, object>();
                                                HelperObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, httpContext);
                                                if (HelperObject == null)
                                                {
                                                    SessionException exception = new SessionException();
                                                    exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                                                    exception.ExceptionCode = EFlags.ClassNotFount;
                                                    throw exception;
                                                }
                                                else
                                                {
                                                    if (HelperObject != null)
                                                        Field.SetValue(NewObject, HelperObject);
                                                }
                                            }
                                        }
                                        attrindex++;
                                    }
                                }
                                else
                                {
                                    HelperObject = null;
                                    int attrindex = 0;
                                    var Properties = ((System.Reflection.TypeInfo)ParameterType).DeclaredProperties;
                                    while (attrindex < fieldAttributes.Attributes.Count)
                                    {
                                        if (fieldAttributes.Attributes[attrindex] == "Autowired")
                                        {
                                            var Property = Properties.Where(x => x.Name == fieldAttributes.DeclaredName).FirstOrDefault();
                                            if (Property != null)
                                            {
                                                ClassName = fieldAttributes.Name;
                                                CreatedObjects = new Dictionary<string, object>();
                                                HelperObject = container.LowlevelBeanCreation(ClassName, CreatedObjects, null, httpContext);
                                                if (HelperObject == null)
                                                {
                                                    SessionException exception = new SessionException();
                                                    exception.SetMessage(exception.GenerateMessage(EFlags.BeanNotCreated));
                                                    exception.ExceptionCode = EFlags.ClassNotFount;
                                                    throw exception;
                                                }
                                                else
                                                {
                                                    if (HelperObject != null)
                                                        Property.SetValue(NewObject, HelperObject);
                                                }
                                            }
                                        }
                                        attrindex++;
                                    }
                                }
                                fieldIndex++;
                            }
                        }
                    }
                }
                else
                {
                    SessionException exception = new SessionException();
                    exception.SetMessage(exception.GenerateMessage(EFlags.ClassNotFount));
                    exception.ExceptionCode = EFlags.ClassNotFount;
                    throw exception;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NewObject;
        }

        #endregion


        #region DYNAMIC OBJECT CREATION

        public IDictionary<string, GraphContainerModal> GetAllBeanType()
        {
            return container.GetGraphContainerModalCollection();
        }

        public void BuildServices(IServiceCollection services, List<string> Namespaces, bool EnableMySql, bool EnableMSSql, string ConnectionString)
        {
            this.container.SetConnectionString(ConnectionString);
            if (EnableMSSql)
                services.AddScoped<IDb, BottomhalfCore.DatabaseLayer.MsSql.Code.Db>(options =>
                {
                    return new BottomhalfCore.DatabaseLayer.MsSql.Code.Db(ConnectionString);
                });
            else if (EnableMySql)
                services.AddScoped<IDb, BottomhalfCore.DatabaseLayer.MySql.Code.Db>(options =>
                {
                    return new BottomhalfCore.DatabaseLayer.MySql.Code.Db(ConnectionString);
                });

            BuildServices(services, Namespaces);
        }

        public void BuildServices(IServiceCollection services, List<string> Namespaces)
        {
            services.AddTransient<IConvertToDataSet<ConvertToDataSet>, ConvertToDataSet>();
            services.AddTransient<TableAutoMapper>();
            services.AddTransient<IValidateModal<ValidateModal>, ValidateModal>();
            services.AddTransient<APIManagerModal>();
            services.AddTransient<ApplicationDetail>();
            services.AddTransient<CurrentRequestDetail>();
            services.AddTransient<ServiceResult>();

            Type classType = null;
            string InjectionTypeName = string.Empty;
            if (context != null)
            {
                if (Namespaces != null && Namespaces.Count > 0)
                {
                    int typeIndex = 0;
                    var allType = context.GetAllBeanType();
                    while (typeIndex < Namespaces.Count)
                    {
                        var types = allType.Where(x => x.Key == Namespaces.ElementAt(typeIndex).ToLower()).SelectMany(i => i.Value.TypeDetail).ToList();
                        if (types != null && types.Count > 0)
                        {
                            foreach (var typeDetail in types.Select(x => x.Value))
                            {
                                if (typeDetail.IsSingleTon)
                                {
                                    classType = Type.GetType(typeDetail.AssemblyQualifiedName);
                                    if (classType != null)
                                        services.AddSingleton(classType);
                                    classType = null;
                                }
                                else if (typeDetail.IsScoped)
                                {
                                    classType = Type.GetType(typeDetail.AssemblyQualifiedName);
                                    if (classType != null)
                                        services.AddScoped(classType);
                                    classType = null;
                                }
                                else
                                {
                                    classType = Type.GetType(typeDetail.AssemblyQualifiedName);
                                    if (classType != null)
                                        services.AddTransient(classType);
                                    classType = null;
                                }
                            }
                        }
                        typeIndex++;
                    }
                }
                else
                {
                    foreach (var itemType in context.GetAllBeanType())
                    {
                        foreach (var typeDetail in itemType.Value.TypeDetail)
                        {
                            classType = Type.GetType(typeDetail.Value.AssemblyQualifiedName);
                            if (classType != null)
                                services.AddTransient(classType);
                            classType = null;
                        }
                    }
                }

                foreach (var itemType in context.GetAllBeanType())
                {
                    if (Namespaces != null && Namespaces.Count > 0)
                    {
                        foreach (var typeDetail in itemType.Value.TypeDetail)
                        {
                            int index = 0;
                            if (typeDetail.Value.AnnotationNames != null && typeDetail.Value.ClassFullyQualifiedName == "")
                            {
                                while (index < typeDetail.Value.AnnotationNames.Count)
                                {
                                    InjectionTypeName = typeDetail.Value.AnnotationNames.ElementAt(index).AnnotationName;
                                    if (InjectionTypeName == Constants.Transient)
                                    {
                                        classType = Type.GetType(typeDetail.Value.AssemblyQualifiedName);
                                        if (classType != null)
                                            services.AddTransient(classType);
                                        classType = null;
                                        break;
                                    }
                                    else if (InjectionTypeName == Constants.Scoped)
                                    {
                                        classType = Type.GetType(typeDetail.Value.AssemblyQualifiedName);
                                        if (classType != null)
                                            services.AddScoped(classType);
                                        classType = null;
                                        break;
                                    }
                                    else if (InjectionTypeName == Constants.SingleTon)
                                    {
                                        classType = Type.GetType(typeDetail.Value.AssemblyQualifiedName);
                                        if (classType != null)
                                            services.AddSingleton(classType);
                                        classType = null;
                                        break;
                                    }
                                    else
                                    {
                                        classType = Type.GetType(typeDetail.Value.AssemblyQualifiedName);
                                        if (classType != null)
                                            services.AddTransient(classType);
                                        classType = null;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var typeDetail in itemType.Value.TypeDetail)
                        {
                            //if (typeDetail.Value.AssemblyName.IndexOf("BottomhalfCore,") == -1)
                            //{
                            classType = Type.GetType(typeDetail.Value.AssemblyQualifiedName);
                            if (classType != null)
                                services.AddTransient(classType);
                            classType = null;
                            //}
                        }
                    }
                }
            }
        }

        public Object GetDictonary(Type KeyType, Type ValueType)
        {
            Type dictClone = typeof(Dictionary<,>);
            Type[] dictonaryArgs = { KeyType, ValueType };
            Type dictonary = dictClone.MakeGenericType(dictonaryArgs);
            Object NewDictonary = Activator.CreateInstance(dictonary);
            return NewDictonary;
        }

        public Object GetList(Type ValueType)
        {
            Type genListType = typeof(List<>);
            Type[] listArgType = { ValueType };
            Type NewList = genListType.MakeGenericType(listArgType);
            Object NewListObject = Activator.CreateInstance(NewList);
            return NewListObject;
        }

        public DataSet ConvertToDataSet<T>(IList<T> ComplexObjectRecord)
        {
            IConvertToDataSet<ConvertToDataSet> converterToDataSet = new ConvertToDataSet();
            return converterToDataSet.ToDataSet<T>(ComplexObjectRecord);
        }

        #endregion

        #region CONFIGURATION DETAIL


        #endregion

        #region MANAGE SESSION OBJECT

        public string AddNewSession(string UniqueCombinationString, string Key, Object Value, string Token)
        {
            if (string.IsNullOrEmpty(Token))
                Token = ComputeSha256Hash(UniqueCombinationString);
            if (container.Set(Token, Key, Value))
                return Token;
            else
                return null;
        }
        public string GetConnectionString()
        {
            return container.ConnectionString;
        }

        #endregion

        #region COMMON SERVICE

        public ServiceResult ValidateModal(Type ObjectName, dynamic ReferencedObject)
        {
            IValidateModal<ValidateModal> validateModal = new ValidateModal();
            ServiceResult serviceResult = validateModal.ValidateModalFieldsService(ObjectName, ReferencedObject);
            return serviceResult;
        }

        public string GetTokenName()
        {
            return this.container.AccessTokenName;
        }

        public EFlags ValidateToken(string Key)
        {
            return this.container.ValidateToken(Key);
        }

        public Boolean IsValidTable(DataTable Grid)
        {
            Boolean Flag = false;
            if (Grid != null)
            {
                if (Grid.Columns.Count > 0)
                    Flag = true;
            }
            return Flag;
        }

        public Boolean IsValidDataSet(DataSet Grid)
        {
            Boolean Flag = false;
            if (Grid != null)
            {
                if (Grid.Tables.Count > 0)
                {
                    int index = 0;
                    while (index < Grid.Tables.Count)
                    {
                        if (!IsValidTable(Grid.Tables[index]))
                            break;
                        index++;
                    }
                }
                Flag = true;
            }
            return Flag;
        }

        #endregion

        public void SetApplicationDetail(string ApplicationName, string ContentRootPath, string Env)
        {
            applicationDetail = new ApplicationDetail();
            applicationDetail.ApplicationName = ApplicationName;
            applicationDetail.ContentRootPath = ContentRootPath;
            applicationDetail.EnvironmentName = Env;
        }

        public string GetContentRootPath()
        {
            string Path = null;
            if (this.applicationDetail != null)
                Path = this.applicationDetail.ContentRootPath;
            return Path;
        }

        public BeanContext SetTokenName(string TokenName)
        {
            this.container.SetTokenName(TokenName);
            return this;
        }

        public bool RemoveToken(string Token)
        {
            return this.container.Remove(Token);
        }

        #region DiOperations

        public void AddScoped<T>()
        {
            DiQueue diQueue = DiQueue.GetInstance();
            diQueue.AddScope<T>();
        }

        public Object GetScopedClass<T>(string UniqueId)
        {
            DiQueue diQueue = DiQueue.GetInstance();
            Object ClassObject = diQueue.GetScoped<T>(UniqueId);
            return ClassObject;
        }

        #endregion
    }
}

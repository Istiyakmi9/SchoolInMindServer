using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.Exceptions;
using BottomhalfCore.Flags;
using BottomhalfCore.IFactoryContext;
using BottomhalfCore.DiService;
using BottomhalfCore.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BottomhalfCore.FactoryContext
{
    public class Container : IContainer
    {
        private string metadata;
        public string FrameWorkName { set; get; }
        private ConcurrentDictionary<string, UserCacheData> MapContext;
        private ConcurrentDictionary<string, GraphContainerModal> GraphContainerModalCollection = null;
        private DiQueue diQueue = null;
        private static Container instance = null;
        private static readonly Object _lock = new object();
        private Boolean IsFreshContainer = false;
        private string _AccessTokenName = default(string);
        private string _RefreshTokenName = default(string);
        private string CS;
        Dictionary<string, List<InterfaceClassLinker>> ObjInterfaceClassLinker = null;
        private ConfigDetail ObjConfigDetail = null;
        private UserCacheData ObjSession = null;
        private List<DocCollector> ObjDocCollectorlst;
        //private List<string> ProjectNames;
        private string ProjectPath { set; get; }
        private string ProjectBinPath { set; get; }
        private IList<string> NoCheckClasses;
        private LogInformationToFile logInformationToFile;
        public bool MSSqlState { set; get; }
        public bool MYSqlState { set; get; }
        public string ConnectionString { get { return CS; } }
        public string AccessTokenName { get { return _AccessTokenName; } }
        public string RefreshTokenName { get { return _RefreshTokenName; } }

        private Container()
        {
            //if (ProjectNames == null) ProjectNames = new List<string>();
            NoCheckClasses = null;
            this.FrameWorkName = "BottomhalfCore";
            MapContext = new ConcurrentDictionary<string, UserCacheData>();
        }

        public void AddSingleton(dynamic ClassObject)
        {

        }

        public void SetSQLType(SqlType sqlType)
        {
            if ((int)sqlType == 1)
            {
                this.MSSqlState = true;
                this.MYSqlState = false;
            }
            else if ((int)sqlType == 2)
            {
                this.MYSqlState = true;
                this.MSSqlState = false;
            }
        }

        //public void AddProjectName(string Name)
        //{
        //    if (!string.IsNullOrEmpty(Name) && this.FrameWorkName != Name)
        //    {
        //        if (ProjectNames.Where(x => x == Name).FirstOrDefault() == null)
        //            ProjectNames.Add(Name);
        //    }
        //}

        //public void AddProjectName(List<string> Namespaces)
        //{
        //    if (Namespaces != null && Namespaces.Count() > 0)
        //        ProjectNames.AddRange(Namespaces);
        //}

        //public List<string> GetProjectName()
        //{
        //    return this.ProjectNames;
        //}

        //public Boolean ProjectNamesIsNotEmpty()
        //{
        //    if (this.ProjectNames == null) return true;
        //    else return !(this.ProjectNames.Count() > 0);
        //}

        private Container(string CurrentFolderPath)
        {
            //if (ProjectNames == null) ProjectNames = new List<string>();
            this.FrameWorkName = "BottomhalfCore";
            NoCheckClasses = null;
            logInformationToFile = new LogInformationToFile(CurrentFolderPath);
            this.SetProjectPath(CurrentFolderPath, null);
            MapContext = new ConcurrentDictionary<string, UserCacheData>();
        }

        public void SetProjectPath(string ProjectPath, string ProjectBinPath)
        {
            this.ProjectPath = ProjectPath;
            this.ProjectBinPath = ProjectBinPath;
        }

        public string GetProjectBinPath()
        {
            return this.ProjectBinPath;
        }

        public string GetProjectPath()
        {
            return this.ProjectPath;
        }

        public void SetProjectDocumentation(List<DocCollector> ObjDocCollectorlst)
        {
            this.ObjDocCollectorlst = ObjDocCollectorlst;
        }

        public List<DocCollector> GetProjectDocumentation()
        {
            return this.ObjDocCollectorlst;
        }

        public static Container GetInstance(string CurrentFolderPath)
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new Container(CurrentFolderPath);
                }
            }

            return instance;
        }

        public static Container GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new Container();
                }
            }

            return instance;
        }

        public void WriteToFile(string Message)
        {
            this.logInformationToFile.WriteArrayToFile(new List<string> { Message }, "AsmFiles");
        }
        public void WriteToFile(List<string> Messages)
        {
            this.logInformationToFile.WriteArrayToFile(Messages, "AsmFiles");
        }
        public void WriteToFile(string Message, string FileName)
        {
            this.logInformationToFile.WriteArrayToFile(new List<string> { Message }, FileName);
        }
        public void WriteToFile(List<string> Messages, string FileName)
        {
            this.logInformationToFile.WriteArrayToFile(Messages, FileName);
        }
        public void WriteToFile(Dictionary<string, TypeRefCollection> refCollection)
        {
            this.logInformationToFile.WriteToFile(refCollection);
        }

        #region SESSION AND TOKEN

        public void SetTokenName(string TokenName)
        {
            if (string.IsNullOrEmpty(this.AccessTokenName))
                this._AccessTokenName = TokenName;
        }

        public void SetRefreshTokenName(string TokenName)
        {
            if (string.IsNullOrEmpty(this.RefreshTokenName))
                this._RefreshTokenName = TokenName;
        }

        #endregion

        #region CONFIGURATION DETAIL SETUP

        //------------ Return web.config value based on key provided by user. ----------------------

        public void SetConnectionString(string CS)
        {
            this.CS = CS;
        }

        public string GetConfigurationConnectionString(string Key)
        {
            try
            {
                string CollectionValue = null;
                IDictionary<string, string> ObjData = null;
                ObjData = this.ObjConfigDetail.ConnectionStringCollection;
                if (Key == null)
                    CollectionValue = ObjData.FirstOrDefault().Value;
                else
                    CollectionValue = ObjData.Where(x => x.Key.ToLower() == Key.ToLower()).FirstOrDefault().Value;
                return CollectionValue;
            }
            catch (Exception)
            {
                throw new ApplicationException("Connection string found. Please check your webconifg.");
            }
        }
        public string GetConfigurationValue(string Key)
        {
            try
            {
                string CollectionValue = null;
                if (Key != null && this.ObjConfigDetail != null)
                    CollectionValue = this.ObjConfigDetail.AppSettingCollection.Where(x => x.Key.ToLower() == Key.ToLower()).FirstOrDefault().Value;
                return CollectionValue;
            }
            catch (Exception)
            {
                throw new ApplicationException("Connection string found. Please check your webconifg.");
            }
        }

        #endregion

        #region COMMON METHODS

        public void AddNoCheckClass(string ClassName)
        {
            if (this.NoCheckClasses == null)
                this.NoCheckClasses = new List<string>();
            this.NoCheckClasses.Add(ClassName);
        }
        public Boolean IsNoCheck(string ClassName)
        {
            Boolean Flag = false;
            if (this.NoCheckClasses != null)
            {
                var NoCheckClassName = this.NoCheckClasses.Where(x => x == ClassName.ToLower());
                if (NoCheckClassName.Count() > 0)
                    Flag = true;
            }
            return Flag;
        }
        public Boolean IsPrimitiveType(Type NewType)
        {
            Boolean Flag = false;
            if (NewType.IsValueType)
                Flag = true;
            else if (NewType == typeof(System.String))
                Flag = true;
            return Flag;
        }

        public void SetGraphContainerModalCollection(IDictionary<string, GraphContainerModal> graphContainerModal)
        {
            if (this.GraphContainerModalCollection == null || this.GraphContainerModalCollection.Count() == 0)
                this.GraphContainerModalCollection = (ConcurrentDictionary<string, GraphContainerModal>)graphContainerModal;
        }
        public Boolean IsReadyContainerInvoked()
        {
            return IsFreshContainer;
        }
        public void ContainerStatus(Boolean ContainerFlag)
        {
            IsFreshContainer = ContainerFlag;
        }
        public void SetMetadata(string metadata)
        {
            this.metadata = metadata;
        }
        public IDictionary<string, GraphContainerModal> GetGraphContainerModalCollection()
        {
            return this.GraphContainerModalCollection;
        }

        public ParameterDetail GetActiveConstructor(TypeRefCollection TypeWithRefList)
        {
            // Retrieve ctor based on annotation if presnet else first one.
            ParameterDetail CurrentCtor = null;
            if (TypeWithRefList.Constructors.Count() > 0)
            {
                int index = 0;
                while (index < TypeWithRefList.Constructors.Count())
                {
                    if (TypeWithRefList.Constructors.ElementAt(index).Value.AnnotationDetail != null)
                    {
                        var ActiveCtor = TypeWithRefList.Constructors.ElementAt(index).Value.AnnotationDetail.Where(x => x.AnnotationName == "Enabled").FirstOrDefault();
                        if (ActiveCtor != null)
                        {
                            if (ActiveCtor.OrderNo == 1)
                                CurrentCtor = TypeWithRefList.Constructors.ElementAt(index).Value;
                        }
                    }
                    index++;
                }

                if (CurrentCtor == null)
                    CurrentCtor = TypeWithRefList.Constructors.ElementAt(0).Value;
            }
            return CurrentCtor;
        }


        public Type FindDataType(string TypeName)
        {
            Type DataType = null;
            if (TypeName == "string" || TypeName == "System.String")
            {
                return DataType = typeof(System.String);
            }
            else if (TypeName == "int" || TypeName == "System.Int16")
            {
                return DataType = typeof(System.Int16);
            }
            else if (TypeName == "int" || TypeName == "System.Int32")
            {
                return DataType = typeof(System.Int32);
            }
            else if (TypeName == "int" || TypeName == "System.Int64")
            {
                return DataType = typeof(System.Int64);
            }
            else if (TypeName == "single" || TypeName == "System.Single")
            {
                return DataType = typeof(System.Single);
            }
            else if (TypeName == "double" || TypeName == "System.Double")
            {
                return DataType = typeof(System.Double);
            }
            else if (TypeName == "decimal" || TypeName == "System.Decimal")
            {
                return DataType = typeof(System.Decimal);
            }
            else if (TypeName == "bool" || TypeName == "System.Boolean")
            {
                return DataType = typeof(System.Boolean);
            }
            else if (TypeName == "datetime" || TypeName == "System.DateTime")
            {
                return DataType = typeof(System.DateTime);
            }
            return DataType;
        }
        private Object GetValue(string TypeName, out Boolean IsLiteral)
        {
            if (TypeName == "string" || TypeName == "System.String")
            {
                IsLiteral = true;
                return "";
            }
            else if (TypeName == "int" || TypeName == "System.Int16" || TypeName == "System.Int32" ||
                TypeName == "System.Int64" || TypeName == "System.Single" || TypeName == "System.Double")
            {
                IsLiteral = true;
                return 0;
            }
            else if (TypeName == "decimal" || TypeName == "System.Decimal")
            {
                IsLiteral = true;
                return 0;
            }
            else if (TypeName == "bool" || TypeName == "System.Boolean")
            {
                IsLiteral = true;
                return false;
            }
            else if (TypeName == "datetime" || TypeName == "System.DateTime")
            {
                IsLiteral = true;
                return DateTime.Now;
            }
            IsLiteral = false;
            return null;
        }


        public void LogError(Exception _exception, string ExtraMessage)
        {
            string ServerErrorLogFolder = string.Empty;
            ServerErrorLogFolder = @"C:\";
            if (!string.IsNullOrEmpty(ServerErrorLogFolder))
            {
                DirectoryInfo ServerLogDirectory = null;
                ServerErrorLogFolder = Path.Combine(ServerErrorLogFolder, "ServerLogs");
                if (!Directory.Exists(ServerErrorLogFolder))
                    ServerLogDirectory = Directory.CreateDirectory(ServerErrorLogFolder);
                else
                    ServerLogDirectory = new DirectoryInfo(ServerErrorLogFolder);
                if (ServerLogDirectory.Exists)
                {
                    ServerLogDirectory = null;
                    string CurrentLogFolder = Path.Combine(ServerErrorLogFolder, DateTime.Now.ToShortDateString().Replace(@"/", "-"));
                    if (!Directory.Exists(CurrentLogFolder))
                        ServerLogDirectory = Directory.CreateDirectory(CurrentLogFolder);
                    else
                        ServerLogDirectory = new DirectoryInfo(CurrentLogFolder);
                    if (ServerLogDirectory.Exists)
                    {
                        string NewLogFile = $"E-{this.FrameWorkName}Errorfile-On" + "__" + DateTime.Now.ToShortTimeString().Replace(":", "-").Replace(" ", "") + ".txt";
                        if (File.Exists(Path.Combine(CurrentLogFolder, NewLogFile)))
                            NewLogFile = NewLogFile + "_1";
                        NewLogFile = Path.Combine(CurrentLogFolder, NewLogFile);
                        FileStream _ObjFile = File.Create(NewLogFile);
                        using (StreamWriter _ObjWriter = new StreamWriter(_ObjFile))
                        {
                            string FullMessage = "ExtraMessage: " + ExtraMessage + "\nException message: " + _exception.Message + "\nInner exception message: "
                                + _exception.InnerException + "\nStack trace: " + _exception.StackTrace;
                            _ObjWriter.WriteLine(FullMessage);
                        }
                    }
                }
            }
        }
        #endregion


        #region OPERATION ON MAP

        public EFlags ValidateToken(string Key)
        {
            return instance.ValidateCurrentRequestToken(Key);
        }
        private EFlags ValidateCurrentRequestToken(string Key)
        {
            ObjSession = null;
            ((ConcurrentDictionary<string, UserCacheData>)MapContext).TryGetValue(Key, out ObjSession);
            if (ObjSession != null)
            {
                Double TotalSeconds = (DateTime.Now - ObjSession.LastUpdatedOn).TotalSeconds;
                if (TotalSeconds > 1200)
                {
                    MapContext.TryRemove(Key, out ObjSession);
                    return EFlags.TokenExpired;
                }
                else
                {
                    ObjSession.LastUpdatedOn = DateTime.Now;
                }
            }
            else
            {
                return EFlags.TokenNotFound;
            }
            return EFlags.Success;
        }
        public Boolean Set(string Token, string ConfigConnectionString)
        {
            return Set(Token, null, null, ConfigConnectionString);
        }
        public Boolean Set(string Token, string Key, Object UserObject)
        {
            return Set(Token, Key, UserObject, null);
        }
        public Boolean Set(string Token, string Key, Object UserObject, string CurrentSessionConnectionString)
        {
            DataByKey ObjDataByKey = null;
            Boolean Flag = false;
            ObjSession = null;
            ((ConcurrentDictionary<string, UserCacheData>)MapContext).TryGetValue(Token, out ObjSession);
            if (ObjSession != null)
            {
                if (!string.IsNullOrEmpty(CurrentSessionConnectionString))
                    ObjSession.SessionConnectionString = CurrentSessionConnectionString;
                ObjSession.LastUpdatedOn = DateTime.Now;
                if (UserObject != null && Key != null)
                {
                    ObjDataByKey = MapContext[Token].ObjDataByKey.Where(x => x.Key == Key).FirstOrDefault();
                    if (ObjDataByKey != null)
                        ObjDataByKey.Value = UserObject;
                    else
                    {
                        ObjDataByKey = new DataByKey
                        {
                            Key = Key,
                            Value = UserObject
                        };
                        MapContext[Token].ObjDataByKey.Add(ObjDataByKey);
                    }
                }
                Flag = true;
            }
            else
            {
                UserCacheData ObjUserObject = new UserCacheData
                {
                    LastUpdatedOn = DateTime.Now,
                    SessionConnectionString = CurrentSessionConnectionString
                };
                if (Key != null && UserObject != null)
                {
                    ObjDataByKey = new DataByKey
                    {
                        Key = Key,
                        Value = UserObject
                    };
                    ObjUserObject.ObjDataByKey.Add(ObjDataByKey);
                }
                MapContext.TryAdd(Token, ObjUserObject);
                Flag = true;
            }
            return Flag;
        }
        public String Add(string HashKey, string Key, Object UserObject, string ConnectionString)
        {
            DateTime TimeStamp = DateTime.Now;
            if (!Set(HashKey, Key, UserObject, ConnectionString))
                HashKey = null;
            return HashKey;
        }
        public String Update(string Token, string Key, Object UserObject, string ConnectionString)
        {
            String GeneratedToken = null;
            DateTime TimeStamp = DateTime.Now;
            if (!Set(Token, Key, UserObject, ConnectionString))
                GeneratedToken = "Success";
            return GeneratedToken;
        }
        public static void FireException(string Message, EFlags ExceptionCode, string Token)
        {
            SessionException exception = new SessionException(ExceptionCode);
            exception.SetMessage(Message);
            exception.Token = Token;
            exception.ExceptionCode = ExceptionCode;
            throw exception;
        }
        public Boolean Remove(string Token)
        {
            Boolean Flag = false;
            UserCacheData RemovedUserCacheData = null;
            if (instance.MapContext.Count() > 0)
            {
                Flag = instance.MapContext.TryRemove(Token, out RemovedUserCacheData);
            }
            else
            {
                Flag = true;
            }
            return Flag;
        }
        public Object Get(string Token, string Key)
        {
            Object userDetail = null;
            ObjSession = null;
            if (Key != null)
            {
                ((ConcurrentDictionary<string, UserCacheData>)MapContext).TryGetValue(Token, out ObjSession);
                if (ObjSession != null)
                {
                    DataByKey ObjDataByKey = ObjSession.ObjDataByKey.Where(x => x.Key == Key).FirstOrDefault();
                    if (ObjDataByKey != null)
                        userDetail = ObjDataByKey.Value;
                }
            }
            return userDetail;
        }
        public Boolean SetConnectionString(string Token, string ConnectionString)
        {
            Boolean Flag = false;
            ObjSession = null;
            ((ConcurrentDictionary<string, UserCacheData>)MapContext).TryGetValue(Token, out ObjSession);
            if (ObjSession != null)
            {
                ObjSession.SessionConnectionString = ConnectionString;
                MapContext[Token] = ObjSession;
            }
            else
            {
                throw new ApplicationException("Token not found");
            }
            return Flag;
        }
        public string GetConnectionString(string Token)
        {
            string ConnectionString = null;
            ObjSession = null;
            ((ConcurrentDictionary<string, UserCacheData>)MapContext).TryGetValue(Token, out ObjSession);
            if (ObjSession != null)
            {
                ConnectionString = ObjSession.SessionConnectionString;
            }
            return ConnectionString;
        }
        public void Logout(string CurrentSession)
        {
            ObjSession = null;
            if (CurrentSession != "" && CurrentSession != null)
            {
                ((ConcurrentDictionary<string, UserCacheData>)MapContext).TryRemove(CurrentSession, out ObjSession);
                if (ObjSession != null)
                {
                    // Log user detail
                }
            }
        }
        public bool ContainersKey(string key)
        {
            return MapContext.ContainsKey(key);
        }
        public Boolean CleanUp()
        {
            Boolean Flag = false;
            // Perform some clean up activity
            return Flag;
        }

        #endregion


        #region BEAN OPERATION

        private string GetImplementationType(string GenericName)
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
        public Dictionary<string, Type> ReMapInnerTypes(Type InterfaceType, Type ClassType)
        {
            int Total = 0;
            string TypeName = null;
            TypeRefCollection TypeCollection = null;
            Dictionary<string, Type> ParamGenericTypeDetail = null;
            Total = InterfaceType.GenericTypeArguments.Count();
            if (Total > 0)
            {
                ParamGenericTypeDetail = new Dictionary<string, Type>();
                int i = 0;
                while (i < Total)
                {
                    TypeName = InterfaceType.GenericTypeArguments[i].Name;
                    TypeCollection = GetTypeRefByName(TypeName);
                    if (TypeCollection != null)
                    {
                        TypeName = null;
                        if (((TypeInfo)ClassType).GenericTypeParameters.Count() > 0)
                        {
                            TypeName = ((TypeInfo)ClassType).GenericTypeParameters[i].Name;
                            ParamGenericTypeDetail.Add(TypeName, Type.GetType(TypeCollection.AssemblyQualifiedName));
                        }
                    }
                    else
                        throw BuildBeanException("Type detail not found inside the container for : " + TypeName, "", "ReMapInnerTypes");
                    i++;
                }
            }
            return ParamGenericTypeDetail;
        }
        private string ReadPropsFile(string FileName, string Fieldname)
        {
            string ActualData = null;
            //if (FileName.IndexOf(".json") == -1)
            //    FileName = FileName + ".json";
            //string CurrentFilePath = Path.Combine(this.ProjectPath, "props", FileName);
            //if (File.Exists(CurrentFilePath))
            //{
            //    using (StreamReader reader = new StreamReader(CurrentFilePath))
            //    {
            //        string Data = reader.ReadToEnd();
            //        if (!string.IsNullOrEmpty(Data))
            //        {
            //            dynamic FormatterData = JsonConvert.DeserializeObject(Data);
            //            ActualData = FormatterData[Fieldname];
            //        }
            //    }
            //}
            return ActualData;
        }

        public TypeRefCollection GetTypeRefByName(string FullName)
        {
            try
            {
                TypeRefCollection TypeWithRefObject = null;
                if (FullName.IndexOf(".") != -1)
                {
                    string QualifiedName = FullName.Substring(0, FullName.LastIndexOf("."));
                    string TypeName = FullName.Substring(FullName.LastIndexOf('.') + 1, FullName.Length - FullName.LastIndexOf('.') - 1);
                    if (!string.IsNullOrEmpty(QualifiedName))
                    {
                        GraphContainerModalCollection.TryGetValue(QualifiedName.ToLower(), out GraphContainerModal graphContainerModal);
                        if (graphContainerModal != null)
                            graphContainerModal.TypeDetail.TryGetValue(TypeName.ToLower(), out TypeWithRefObject);
                        else
                        {
                            if (QualifiedName.IndexOf("Bottomhalf") != -1)
                            {
                                if (FullName == "Microsoft.AspNetCore.Http.HttpContext")
                                    return null;
                                else if (FullName == "BottomhalfCore.FactoryContext.BeanContext")
                                    return null;

                                TypeWithRefObject = new TypeRefCollection();
                                Type RuntimeType = Type.GetType(FullName);
                                if (RuntimeType != null)
                                {
                                    TypeWithRefObject.ClassName = TypeName;
                                    TypeWithRefObject.ClassFullyQualifiedName = QualifiedName;
                                    TypeWithRefObject.AssemblyQualifiedName = RuntimeType.AssemblyQualifiedName;
                                    //TypeWithRefObject.ClassType = RuntimeType;
                                }
                                else
                                {
                                    throw BuildBeanException("Type: " + FullName, QualifiedName + " assembly is not scanned by framework.", "LowlevelBeanCreation");
                                }
                            }
                            else
                            {
                                throw BuildBeanException("Type: " + FullName, QualifiedName + " assembly is not scanned by framework.", "LowlevelBeanCreation");
                            }
                        }
                    }
                }
                else
                {
                    throw BuildBeanException("Type Name: " + FullName + " not found.", "", "GetTypeRefByName");
                }
                return TypeWithRefObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Object LowlevelBeanCreation(string BeanId, Dictionary<string, Object> CreatedObjects,
            Dictionary<string, Type>
            ParamGenericTypeDetail,
            HttpContext httpContext)
        {
            TypeRefCollection TypeWithRefList = null;
            TypeRefCollection TempRef = null;
            List<Type> ParamType = null;
            List<Object> TypeObject = null;
            List<Object> GenericTypeObject = null;
            ParameterDetail ParamDetail = null;
            Type CurrentType = null;
            Type NewCurrentType = null;
            ParameterNameCollection argument = null;
            Boolean IsLiteral = false;
            Boolean IsDirectType = false;
            Object NewObject = null;
            string ImplementedTypeName = null;
            try
            {
                if (BeanId == "Microsoft.AspNetCore.Http.HttpContext")
                    return httpContext;
                else if (BeanId == "BottomhalfCore.FactoryContext.BeanContext")
                    Type.GetType(BeanId);

                TypeWithRefList = GetTypeRefByName(BeanId);
                if (TypeWithRefList == null)
                {
                    if (BeanId == "Microsoft.AspNetCore.Http.HttpContext")
                        return httpContext;
                    else if (BeanId == "BottomhalfCore.FactoryContext.BeanContext")
                        return BeanContext.GetInstance();
                    else
                        throw BuildBeanException("Type: " + BeanId + " not found. Assembly is not scanned by framework.", "", "LowlevelBeanCreation");
                }
                //if (TypeWithRefList == null)
                //{
                //    if (ParamGenericTypeDetail != null)
                //    {
                //        CurrentType = ParamGenericTypeDetail.Where(x => x.Key == BeanId).FirstOrDefault().Value;
                //        IsDirectType = true;
                //        if (CurrentType == null)
                //            throw BuildBeanException("Type Name: " + argument.Name,
                //                BeanId + " class is a generic type class and expecting parameter of their generic type. But found less or no parameter.",
                //                "LowlevelBeanCreation"
                //            );

                //        ImplementedTypeName = CurrentType.Name;
                //        if (CurrentType.IsGenericType)
                //        {
                //            string ImplName = "";
                //            if (CurrentType.IsInterface)
                //            {
                //                ImplName = CurrentType.FullName.Split(new string[] { "[[" }, StringSplitOptions.RemoveEmptyEntries)[0];
                //                var GenTypeList = this.ObjInterfaceClassLinker.Where(x => x.Key == ImplName).FirstOrDefault().Value;
                //                if (GenTypeList != null && GenTypeList.Count() > 0)
                //                    ImplementedTypeName = GenTypeList[0].Name;
                //            }
                //            TypeWithRefList = GetTypeRefByName(ImplementedTypeName);
                //            if (TypeWithRefList == null)
                //                throw BuildBeanException("Type : " + CurrentType.FullName + " not found.", "", "LowlevelBeanCreation");
                //            var NewParamGenericTypeDetail = ReMapInnerTypes(CurrentType, TypeWithRefList.ClassType);
                //            NewObject = LowlevelBeanCreation(ImplementedTypeName, CreatedObjects, NewParamGenericTypeDetail, httpContext);
                //            return NewObject;
                //        }
                //        else
                //            throw BuildBeanException("Type Name: " + argument.Name, BeanId + " assembly is not scanned by framework", "LowlevelBeanCreation");
                //    }

                //    TypeWithRefList = GetTypeRefByName(ImplementedTypeName);
                //    if (TypeWithRefList == null)
                //        throw BuildBeanException("Implementation type: " + CurrentType.FullName + " not found.", "", "LowlevelBeanCreation");
                //}
                if (TypeWithRefList.ClassFullyQualifiedName != null)
                {
                    if (CreatedObjects.Where(x => x.Key == TypeWithRefList.ClassFullyQualifiedName).FirstOrDefault().Value != null)
                        return CreatedObjects.Where(x => x.Key == TypeWithRefList.ClassFullyQualifiedName).FirstOrDefault().Value;
                    if (CurrentType == null)
                        //CurrentType = TypeWithRefList.ClassType;
                        CurrentType = Type.GetType(TypeWithRefList.AssemblyQualifiedName);

                    if (httpContext != null)
                    {
                        diQueue = DiQueue.GetInstance();
                        NewObject = this.diQueue.GetScoped(CurrentType, httpContext.TraceIdentifier);
                        if (NewObject != null)
                            return NewObject;
                    }
                }
                else
                    throw BuildBeanException("Type : " + BeanId + " not found", "", "LowlevelBeanCreation");
                if (TypeWithRefList != null)
                {
                    TypeObject = new List<Object>();
                    if (TypeWithRefList.Constructors.Count > 0)
                    {
                        ParamDetail = GetActiveConstructor(TypeWithRefList);
                        if (ParamDetail != null)
                        {
                            if (ParamDetail.Parameters.Count > 0)
                            {
                                int index = 0;
                                while (index < ParamDetail.Parameters.Count)
                                {
                                    argument = null;
                                    if (ParamDetail.Parameters.ElementAt(index) != null)
                                    {
                                        //ParameterList = ParamDetail.Parameters.ElementAt(index).Value;
                                        argument = ParamDetail.Parameters.ElementAt(index);
                                        ParamType = new List<Type>();

                                        //argument = ParameterList[paramindex];
                                        if (argument.Name.IndexOf("`") != -1)
                                        {
                                            if (argument.Name.IndexOf("System.Collections") != -1)
                                            {
                                                argument.Name = GetImplementationType(argument.Name);
                                                NewCurrentType = Type.GetType(argument.Name);
                                                if (NewCurrentType == null && ParamDetail.IsGeneric)
                                                {

                                                }
                                                NewObject = Activator.CreateInstance(NewCurrentType);
                                                if (ParamDetail.AnnotationDetail != null)
                                                {
                                                    AnnotationDefination FileAnnotation = ParamDetail.AnnotationDetail.Where(x => x.AnnotationName == "File").FirstOrDefault();
                                                    if (FileAnnotation != null)
                                                    {
                                                        var FileData = ReadPropsFile(FileAnnotation.Filename, argument.ArgumentName);
                                                        if (FileData != null)
                                                        {
                                                            Object[] ArgsData = null; // FileData.ToObject<object[]>();
                                                            int ArgDataIndex = 0;
                                                            while (ArgDataIndex < ArgsData.Count())
                                                            {
                                                                NewObject.GetType().GetMethod("Add").Invoke(NewObject, new[] { ArgsData[ArgDataIndex] });
                                                                ArgDataIndex++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int fieldindex = 0;
                                                NewObject = null;
                                                List<Type> GenericObject = null;
                                                BottomhalfModel.FieldAttributes Field = null;
                                                if (TypeWithRefList.FieldAttributesCollection != null && TypeWithRefList.FieldAttributesCollection.Count > 0)
                                                {
                                                    while (fieldindex < TypeWithRefList.FieldAttributesCollection.Count)
                                                    {
                                                        NewCurrentType = null;
                                                        Field = TypeWithRefList.FieldAttributesCollection[fieldindex];
                                                        if (Field.FieldName != null)
                                                        {
                                                            NewObject = LowlevelBeanCreation(Field.FieldName, CreatedObjects, ParamGenericTypeDetail, httpContext);
                                                            var ExistingObject = CreatedObjects.Where(x => x.Key == NewObject.GetType().FullName).FirstOrDefault();
                                                            if (ExistingObject.Value == null)
                                                                CreatedObjects.Add(NewObject.GetType().FullName, NewObject);
                                                        }
                                                        else
                                                        {
                                                            if (Field.GenericList != null && Field.GenericList.Count > 0)
                                                            {
                                                                int itemindex = 1;
                                                                while (itemindex < Field.GenericList.Count)
                                                                {
                                                                    if (Field.GenericList[itemindex].IndexOf("System.Collections") == -1 && Field.GenericList[itemindex].IndexOf("`") == -1)
                                                                        NewCurrentType = Type.GetType(GetTypeRefByName(Field.GenericList[itemindex]).AssemblyQualifiedName);
                                                                    else
                                                                        NewCurrentType = Type.GetType(Field.GenericList[itemindex]);

                                                                    if (NewCurrentType != null)
                                                                    {
                                                                        if (GenericObject == null)
                                                                            GenericObject = new List<Type>();
                                                                        GenericObject.Add(NewCurrentType);
                                                                    }
                                                                    itemindex++;
                                                                }

                                                                TempRef = null;
                                                                TempRef = GetTypeRefByName(Field.GenericList[0]);
                                                                if (TempRef != null)
                                                                {
                                                                    Type GenericType = null;
                                                                    GenericType = Type.GetType(TempRef.AssemblyQualifiedName).MakeGenericType(GenericObject.ToArray<Type>());
                                                                    NewObject = Activator.CreateInstance(GenericType);
                                                                }
                                                            }
                                                        }
                                                        fieldindex++;
                                                    }
                                                }
                                                else if (argument.ParameterStructurInfo != null)
                                                {
                                                    try
                                                    {
                                                        NewCurrentType = CreateGenericObject(argument, ParamGenericTypeDetail);
                                                    }
                                                    catch (BeanException oBeanException)
                                                    {
                                                        throw oBeanException;
                                                    }
                                                    NewObject = Activator.CreateInstance(NewCurrentType);
                                                    if (!NewCurrentType.ContainsGenericParameters && NewCurrentType.GenericTypeArguments.Count() == 0)
                                                    {
                                                        var ObjectExists = CreatedObjects.Where(x => x.Key == NewCurrentType.FullName).FirstOrDefault();
                                                        if (ObjectExists.Value == null)
                                                            CreatedObjects.Add(NewCurrentType.FullName, NewObject);
                                                    }
                                                }
                                            }
                                            TypeObject.Add(NewObject);
                                        }
                                        else
                                        {
                                            NewObject = GetValue(argument.Name, out IsLiteral);
                                            if (!IsLiteral)
                                            {
                                                //argument.Name = argument.Name.Substring(argument.Name.LastIndexOf('.') + 1, argument.Name.Length - argument.Name.LastIndexOf('.') - 1);
                                                NewObject = LowlevelBeanCreation(argument.Name, CreatedObjects, ParamGenericTypeDetail, httpContext);
                                                var ExistingObject = CreatedObjects.Where(x => x.Key == NewObject.GetType().FullName).FirstOrDefault();
                                                if (ExistingObject.Value == null)
                                                    CreatedObjects.Add(NewObject.GetType().FullName, NewObject);
                                                // throw unable to recognize the parameter type either it is not in the assembly or getting some type creation error.
                                                if (NewObject == null)
                                                    throw BuildBeanException("Type : " + argument.Name + " not found", "", "LowlevelBeanCreation");
                                            }
                                            TypeObject.Add(NewObject);
                                        }
                                    }
                                    else
                                    {
                                        // throw unable to recognize the parameter type either it is not in the assembly or getting some type creation error.
                                        throw BuildBeanException("Type Name: " + argument.Name, "", "LowlevelBeanCreation()");
                                    }
                                    index++;
                                }

                                if (TypeObject.Count > 0)
                                {
                                    if (CurrentType.ContainsGenericParameters)
                                    {
                                        Type[] GenericTypes = new Type[ParamGenericTypeDetail.Count()];
                                        int genericpassedparameter = 0;
                                        while (genericpassedparameter < ParamGenericTypeDetail.Count())
                                        {
                                            GenericTypes[genericpassedparameter] = ParamGenericTypeDetail.ElementAt(genericpassedparameter).Value;
                                            genericpassedparameter++;
                                        }
                                        CurrentType = CurrentType.MakeGenericType(GenericTypes);
                                    }
                                    NewObject = Activator.CreateInstance(CurrentType, TypeObject.ToArray<object>());
                                }
                                else
                                    NewObject = Activator.CreateInstance(CurrentType);

                                if (NewObject.GetType().FullName.IndexOf("System.Collections") == -1)
                                {
                                    var ExistingObject = CreatedObjects.Where(x => x.Key == NewObject.GetType().FullName).FirstOrDefault();
                                    if (ExistingObject.Value == null)
                                        CreatedObjects.Add(NewObject.GetType().FullName, NewObject);
                                }
                            }
                            else
                            {
                                // create object insert into list<object> and return.
                                NewObject = Activator.CreateInstance(CurrentType);
                                TypeObject.Add(NewObject);
                                var ExistingObject = CreatedObjects.Where(x => x.Key == NewObject.GetType().FullName).FirstOrDefault();
                                if (ExistingObject.Value == null)
                                    CreatedObjects.Add(NewObject.GetType().FullName, NewObject);
                            }
                        }
                    }
                    else
                    {
                        if (TypeWithRefList.IsContainsGenericParameters)
                        {
                            int GenericTypeIndex = 0;
                            GenericTypeObject = new List<Object>();
                            if (TypeWithRefList.GenericTypeParamName != null)
                            {
                                while (GenericTypeIndex < TypeWithRefList.GenericTypeParamName.Count)
                                {
                                    NewObject = LowlevelBeanCreation(TypeWithRefList.GenericTypeParamName[GenericTypeIndex], CreatedObjects, ParamGenericTypeDetail, httpContext);
                                    KeyValuePair<string, object> GenericObjects = CreatedObjects.Where(x => x.Key == NewObject.GetType().FullName).FirstOrDefault();
                                    if (GenericObjects.Value == null)
                                        CreatedObjects.Add(NewObject.GetType().FullName, NewObject);
                                    TypeObject.Add(NewObject);
                                    GenericTypeIndex++;
                                }
                            }
                            else
                            {
                                if (!IsDirectType)
                                {
                                    Type[] GenericTypes = new Type[ParamGenericTypeDetail.Count()];
                                    int genericpassedparameter = 0;
                                    while (genericpassedparameter < ParamGenericTypeDetail.Count())
                                    {
                                        GenericTypes[genericpassedparameter] = ParamGenericTypeDetail.ElementAt(genericpassedparameter).Value;
                                        genericpassedparameter++;
                                    }
                                    CurrentType = CurrentType.MakeGenericType(GenericTypes);
                                }
                                else
                                {
                                    NewObject = LowlevelBeanCreation(ImplementedTypeName, CreatedObjects, ParamGenericTypeDetail, httpContext);
                                }
                            }
                            NewObject = Activator.CreateInstance(CurrentType, TypeObject.ToArray<object>());
                        }
                        else
                        {
                            if (CurrentType.Name == "Db")
                                NewObject = Activator.CreateInstance(CurrentType, new[] { CS });
                            else
                                NewObject = Activator.CreateInstance(CurrentType);
                        }
                        TypeObject.Add(NewObject);
                        var ExistingObject = CreatedObjects.Where(x => x.Key == NewObject.GetType().FullName).FirstOrDefault();
                        if (ExistingObject.Value == null)
                            CreatedObjects.Add(NewObject.GetType().FullName, NewObject);
                    }
                }
                return NewObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Type LowlevelCustomGenericType(List<string> GenericList)
        {
            int index = 0;
            Type ActiveType = null;
            if (GenericList != null && GenericList.Count > 0)
            {
                while (index < GenericList.Count)
                {
                    index++;
                }
            }
            return ActiveType;
        }

        private Type CreateGenericObject(ParameterNameCollection argument, Dictionary<string, Type> ParamGenericTypeDetail)
        {
            string NewInnerGenericClassName = null;
            TypeRefCollection TypeWithRefList = null;
            Type NewType = null;
            int structureinfoindex = 0;
            List<Type> ObjectType = null;
            if (argument.ParameterStructurInfo != null)
            {
                ObjectType = new List<Type>();
                while (structureinfoindex < argument.ParameterStructurInfo.Count)
                {
                    NewInnerGenericClassName = argument.ParameterStructurInfo[structureinfoindex].Name;
                    if (ParamGenericTypeDetail.Where(x => x.Key == NewInnerGenericClassName).FirstOrDefault().Value != null)
                    {
                        NewType = ParamGenericTypeDetail.Where(x => x.Key == NewInnerGenericClassName).FirstOrDefault().Value;
                    }
                    else
                    {
                        if (NewInnerGenericClassName.IndexOf("`") != -1)
                            NewType = CreateGenericObject(argument.ParameterStructurInfo[structureinfoindex], ParamGenericTypeDetail);
                        else
                            NewType = CreateGenericObject(argument.ParameterStructurInfo[structureinfoindex], ParamGenericTypeDetail);
                    }
                    ObjectType.Add(NewType);
                    structureinfoindex++;
                }

                TypeWithRefList = null;
                NewInnerGenericClassName = argument.Name;
                if (NewInnerGenericClassName.IndexOf("List`") != -1)
                {
                    if (ObjectType.Count >= 1)
                    {
                        Type genListType = typeof(List<>);
                        Type[] listArgType = { ObjectType.ElementAt(0) };
                        NewType = genListType.MakeGenericType(listArgType);
                    }
                    else
                        throw BuildBeanException("Type : " + argument.Name + " not found.", "", "CreateGenericObject");
                }
                else if (NewInnerGenericClassName.IndexOf("Dictionary`") != -1)
                {
                    if (ObjectType.Count >= 2)
                    {
                        Type DictionaryType = typeof(Dictionary<,>);
                        Type[] DictionaryTypeArray = { ObjectType.ElementAt(0), ObjectType.ElementAt(1) };
                        NewType = DictionaryType.MakeGenericType(DictionaryTypeArray);
                    }
                    else
                        throw BuildBeanException("Type : " + argument.Name + " not found.", "", "CreateGenericObject");
                }
                else
                {
                    if (ObjectType.Count >= 1)
                    {
                        TypeWithRefList = GetTypeRefByName(NewInnerGenericClassName);
                        //NewType = TypeWithRefList.ClassType;
                        NewType = Type.GetType(TypeWithRefList.AssemblyQualifiedName);
                        NewType = NewType.MakeGenericType(ObjectType.ToArray<Type>());
                    }
                    else
                        throw BuildBeanException("Type : " + argument.Name + " not found.", "", "CreateGenericObject");
                }
            }
            else
            {
                if (ParamGenericTypeDetail.Where(x => x.Key == argument.Name).FirstOrDefault().Value != null)
                {
                    NewType = ParamGenericTypeDetail.Where(x => x.Key == argument.Name).FirstOrDefault().Value;
                }
                else
                {
                    TypeWithRefList = GetTypeRefByName(argument.Name);
                }
                return Type.GetType(TypeWithRefList.AssemblyQualifiedName);
            }
            return NewType;
        }
        public Object GetDictonary(Type KeyType, Type ValueType)
        {
            Type dictClone = typeof(Dictionary<,>);
            Type[] dictonaryArgs = { KeyType, ValueType };
            Type dictonary = dictClone.MakeGenericType(dictonaryArgs);
            Object NewDictonary = Activator.CreateInstance(dictonary);
            return NewDictonary;
        }

        public void SetInterfaceClassLinker(Dictionary<string, List<InterfaceClassLinker>> ObjInterfaceClassLinker)
        {
            this.ObjInterfaceClassLinker = ObjInterfaceClassLinker;
        }
        public List<InterfaceClassLinker> GetImplementedType(string ClassName)
        {
            return this.ObjInterfaceClassLinker.Where(x => x.Key == ClassName).FirstOrDefault().Value;
        }
        public Dictionary<string, List<InterfaceClassLinker>> GetInterfaceClassLinker()
        {
            return this.ObjInterfaceClassLinker;
        }

        #endregion

        private static BeanException BuildBeanException(string TypeName, string AssemblyName, string FullPath)
        {
            BeanException beanException = new BeanException();
            string Message = TypeName + "\n. " + AssemblyName;
            beanException.SetMessage(Message);
            beanException.SetExceptionPath(FullPath);
            return beanException;
        }
    }
}

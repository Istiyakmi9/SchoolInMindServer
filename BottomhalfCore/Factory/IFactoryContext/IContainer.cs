using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.Flags;
using BottomhalfCore.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace BottomhalfCore.IFactoryContext
{
    public interface IContainer
    {
        bool MYSqlState { set; get; }
        bool MSSqlState { get; }
        string AccessTokenName { get; }
        string RefreshTokenName { get; }
        string ConnectionString { get; }
        void SetConnectionString(string CS);
        void SetSQLType(SqlType sqlType);
        string FrameWorkName { get; }
        //void AddProjectName(List<string> Namespaces);
        //Boolean ProjectNamesIsNotEmpty();
        //void AddProjectName(string Name);
        //List<string> GetProjectName();
        void SetMetadata(string metadata);
        Object Get(string Token, string Key);
        bool ContainersKey(string key);
        void SetGraphContainerModalCollection(IDictionary<string, GraphContainerModal> ObjGraphContainerModal);
        IDictionary<string, GraphContainerModal> GetGraphContainerModalCollection();
        Type FindDataType(string TypeName);
        Boolean IsReadyContainerInvoked();
        void ContainerStatus(Boolean ContainerFlag);
        Boolean IsPrimitiveType(Type TypeName);
        Object LowlevelBeanCreation(string BeanId, Dictionary<string, Object> ExistingObjects, Dictionary<string, Type> ParamGenericTypeDetail, HttpContext httpContext);
        void SetInterfaceClassLinker(Dictionary<string, List<InterfaceClassLinker>> ObjInterfaceClassLinker);
        Dictionary<string, List<InterfaceClassLinker>> GetInterfaceClassLinker();
        string GetConfigurationConnectionString(string Key);
        TypeRefCollection GetTypeRefByName(string FullName);
        EFlags ValidateToken(string Key);
        void Logout(string CurrentSession);
        Boolean Set(string Token, string ConfigConnectionString);
        Boolean Set(string Token, string Key, Object UserObject);
        Boolean Set(string Token, string Key, Object UserObject, string CurrentSessionConnectionString);
        string GetConnectionString(string Key);
        ParameterDetail GetActiveConstructor(TypeRefCollection TypeWithRefList);
        List<InterfaceClassLinker> GetImplementedType(string ClassName);
        void SetProjectPath(string ProjectPath, string ProjectBinPath);
        void SetProjectDocumentation(List<DocCollector> ObjDocCollectorlst);
        List<DocCollector> GetProjectDocumentation();
        void LogError(Exception _exception, string ExtraMessage);
        string GetConfigurationValue(string Key);
        void SetTokenName(string TokenName);
        void AddNoCheckClass(string ClassName);
        Boolean IsNoCheck(string ClassName);
        string GetProjectPath();
        string GetProjectBinPath();
        Boolean Remove(string Token);
        void WriteToFile(string Message);
        void WriteToFile(List<string> Messages);
        void WriteToFile(string Message, string FileName);
        void WriteToFile(List<string> Messages, string FileName);
        void WriteToFile(Dictionary<string, TypeRefCollection> refCollection);
    }
}

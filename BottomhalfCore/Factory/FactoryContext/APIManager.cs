using BottomhalfCore.Exceptions;
using BottomhalfCore.Factory.IFactoryContext;
using BottomhalfCore.FactoryContext;
using BottomhalfCore.IFactoryContext;
using BottomhalfCore.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BottomhalfCore.Factory.FactoryContext
{
    public class APIManager : IAPIManagerd<APIManager>
    {
        private List<APIManagerModal> aPIManagerModal = null;
        private readonly ICloneObjectSchema<CloneObjectSchema> cloneObjectSchema;
        private Dictionary<string, List<APIManagerModal>> aPIManager = null;
        private IContainer container;

        public APIManager()
        {
            this.cloneObjectSchema = new CloneObjectSchema();
            this.aPIManager = new Dictionary<string, List<APIManagerModal>>();
            this.container = Container.GetInstance();
        }

        private string GetObjectStructure()
        {
            string Structure = string.Empty;

            return Structure;
        }

        private dynamic CreateInnerObjects(Type InnerType, dynamic NewInstance)
        {
            Type FieldType = null;
            PropertyInfo[] propertys = InnerType.GetProperties();
            Type GenericType = null;
            int Index = 0;
            while (Index < propertys.Length)
            {
                FieldType = propertys.ElementAt(Index).PropertyType;
                if (FieldType != null && FieldType.FullName.IndexOf("System.") == -1)
                {
                    try
                    {
                        propertys.ElementAt(Index).SetValue(NewInstance, Activator.CreateInstance(FieldType));
                    }
                    catch (Exception)
                    {
                        propertys.ElementAt(Index).SetValue(NewInstance, null);
                    }
                }
                else if (FieldType.FullName.IndexOf("System.Collections.") != -1)
                {
                    try
                    {
                        if (FieldType.IsInterface && FieldType.IsGenericType)
                        {
                            if (FieldType.FullName.IndexOf("System.Collections.Generic.IList`1") != -1)
                            {
                                object InnerObject = null;
                                Type[] TypeParameters = FieldType.GetGenericArguments();
                                if (TypeParameters.ElementAt(0).IsClass)
                                {
                                    var TypeCtors = TypeParameters.ElementAt(0).GetConstructors();
                                    if (TypeCtors.Length > 0)
                                    {
                                        object[] Arguments = null;
                                        var Param = TypeCtors.ElementAt(0).GetParameters();
                                        if (Param.Length > 0)
                                        {
                                            int TypeParamIndex = 0;
                                            Arguments = new object[Param.Length];
                                            while (TypeParamIndex < Param.Length)
                                            {
                                                Arguments[TypeParamIndex] = null;
                                                TypeParamIndex++;
                                            }
                                        }
                                        InnerObject = Activator.CreateInstance(TypeParameters.ElementAt(0), Arguments);
                                    }
                                    else
                                        InnerObject = Activator.CreateInstance(TypeParameters.ElementAt(0));
                                }
                                Type ListType = typeof(List<>);
                                GenericType = ListType.MakeGenericType(TypeParameters);
                                object NewObject = Activator.CreateInstance(GenericType);
                                GenericType.GetMethod("Add").Invoke(NewObject, new[] { InnerObject });
                                propertys.ElementAt(Index).SetValue(NewInstance, NewObject);
                            }
                            else if (FieldType.FullName.IndexOf("System.Collections.Generic.IDictionary") != -1)
                            {
                                Type[] TypeParameters = FieldType.GetGenericArguments();
                                Type ListType = typeof(List<>);
                                GenericType = ListType.MakeGenericType(TypeParameters);
                                propertys.ElementAt(Index).SetValue(NewInstance, Activator.CreateInstance(GenericType));
                            }
                        }
                        else
                        {
                            propertys.ElementAt(Index).SetValue(NewInstance, Activator.CreateInstance(FieldType));
                        }
                    }
                    catch (Exception ex)
                    {
                        propertys.ElementAt(Index).SetValue(NewInstance, null);
                    }
                }
                Index++;
            }
            return NewInstance;
        }

        private List<dynamic> ReadParameterType(ParameterInfo[] parameters, bool IsGet)
        {
            List<dynamic> ParameterTypeDetail = new List<dynamic>();
            Type parameterType = null;
            Object NewObject = null;
            Dictionary<string, string> LiteralTypeDetail = null;
            int Index = 0;
            while (Index < parameters.Length)
            {
                parameterType = parameters.ElementAt(Index).ParameterType;
                //cloneObjectSchema.GetEmptyObject(parameterType);

                try
                {
                    if (!IsGet)
                    {
                        //NewObject = Activator.CreateInstance(parameterType);
                        //NewObject = CreateInnerObjects(parameterType, NewObject);
                        NewObject = cloneObjectSchema.GetEmptyObject(parameterType);
                    }
                    else
                    {
                        if (LiteralTypeDetail == null)
                            LiteralTypeDetail = new Dictionary<string, string>();
                        LiteralTypeDetail.Add(parameters.ElementAt(Index).Name, parameterType.Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    NewObject = null;
                }
                Index++;
            }

            if (NewObject != null)
                ParameterTypeDetail.Add(NewObject);
            if (LiteralTypeDetail != null)
                ParameterTypeDetail.Add(LiteralTypeDetail);

            return ParameterTypeDetail;
        }

        private void ReadAttributeAndParameters(Type CurrentType)
        {
            string AttrName = null;
            string RequestType = null;
            string RequestUri = null;
            MethodInfo methodInfo = null;
            IEnumerable<Attribute> attributes = null;
            Attribute attribute = null;
            dynamic Parameter = null;
            MethodInfo[] methodInfos = CurrentType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            string RootRoute = "";
            RouteAttribute routeAttribute = CurrentType.GetCustomAttribute<RouteAttribute>();
            if (routeAttribute != null)
            {
                if (routeAttribute.Template.ToLower().IndexOf("[controller]") != -1)
                    RootRoute = routeAttribute.Template.ToLower().Replace("[controller]", CurrentType.Name.Replace("Controller", "")) + "/";
                else if (routeAttribute.Template.ToLower().IndexOf("{controller}") != -1)
                    RootRoute = routeAttribute.Template.ToLower().Replace("{controller}", CurrentType.Name.Replace("Controller", "")) + "/";
                else
                    RootRoute = routeAttribute.Template + " / ";
            }
            int Index = 0;
            int AttrIndex = 0;
            while (Index < methodInfos.Length)
            {
                RequestUri = null;
                RequestType = null;
                aPIManagerModal = null;
                methodInfo = methodInfos.ElementAt(Index);
                attributes = methodInfo.GetCustomAttributes();
                AttrIndex = 0;
                while (AttrIndex < attributes.Count())
                {
                    attribute = attributes.ElementAt(AttrIndex);
                    AttrName = attribute.GetType().Name;
                    if (AttrName == "HttpPostAttribute")
                    {
                        RequestType = ((HttpPostAttribute)attribute).HttpMethods.FirstOrDefault();
                        RequestUri = ((HttpPostAttribute)attribute).Template;
                    }
                    else if (AttrName == "HttpGetAttribute")
                    {
                        RequestType = ((HttpGetAttribute)attribute).HttpMethods.FirstOrDefault();
                        RequestUri = ((HttpGetAttribute)attribute).Template;
                    }
                    else if (AttrName == "RouteAttribute")
                    {
                        RequestUri = ((RouteAttribute)attribute).Template;
                    }
                    AttrIndex++;
                }

                if (!string.IsNullOrEmpty(RequestType))
                {
                    if (string.IsNullOrEmpty(RequestUri))
                        RequestUri = methodInfo.Name;
                    aPIManagerModal = aPIManager.Where(x => x.Key == CurrentType.Name).FirstOrDefault().Value;
                    Parameter = ReadParameterType(methodInfo.GetParameters(), RequestType.ToLower() == "get");
                    if (aPIManagerModal == null)
                    {
                        aPIManagerModal = new List<APIManagerModal>();
                        aPIManagerModal.Add(new APIManagerModal() { MethodName = RequestType, URL = RootRoute + RequestUri, Parameters = Parameter });
                        aPIManager.Add(CurrentType.Name, aPIManagerModal);
                    }
                    else
                    {
                        aPIManagerModal.Add(new APIManagerModal() { MethodName = RequestType, URL = RootRoute + RequestUri, Parameters = Parameter });
                    }
                }
                Index++;
            }
        }

        public Dictionary<string, List<APIManagerModal>> GetAPIs()
        {
            try
            {
                string Name = null;
                Assembly asm = null;
                List<Assembly> Assemblies = new List<Assembly>();
                string Bindir = container.GetProjectBinPath();
                Dictionary<string, int> LoadedAssemblyNames = new Dictionary<string, int>();
                var AsmFilesArr = Directory.GetFiles(Bindir, "*.dll", SearchOption.AllDirectories);
                var AsmExeFiles = Directory.GetFiles(Bindir, "*.exe", SearchOption.AllDirectories);
                List<string> AsmFiles = AsmFilesArr.Union(AsmExeFiles).ToList<string>();
                string ProjectAssemblyName = Assembly.GetEntryAssembly().GetName().Name + ".dll";
                LoadedAssemblyNames.Add(ProjectAssemblyName, 0);
                string InnerAsm = null;
                string FoundAsmPath = null;
                Type type = null;
                ConstructorInfo Ctor = null;
                ParameterInfo Param = null;
                int ParamIndex = 0;
                int CtorIndex = 0;
                int TypeIndex = 0;
                int Index = 0;
                while (Index < LoadedAssemblyNames.Count())
                {
                    FoundAsmPath = AsmFiles.Where(x => x.Contains(LoadedAssemblyNames.ElementAt(Index).Key)).FirstOrDefault();
                    if (FoundAsmPath != null && LoadedAssemblyNames.ElementAt(Index).Value == 0
                        && LoadedAssemblyNames.ElementAt(Index).Key != this.container.FrameWorkName + ".dll")
                    {
                        asm = null;
                        if (Assemblies.Where(x => x.GetName().Name == Name).FirstOrDefault() == null)
                        {
                            asm = Assembly.LoadFrom(FoundAsmPath);
                            Assemblies.Add(asm);

                            IEnumerable<Type> Types = asm.ExportedTypes;
                            TypeIndex = 0;
                            while (TypeIndex < Types.Count())
                            {
                                type = Types.ElementAt(TypeIndex);
                                if (!type.IsInterface)
                                {
                                    ReadAttributeAndParameters(type);
                                    IEnumerable<ConstructorInfo> Ctors = type.GetConstructors();
                                    CtorIndex = 0;
                                    while (CtorIndex < Ctors.Count())
                                    {
                                        Ctor = Ctors.ElementAt(CtorIndex);
                                        IEnumerable<ParameterInfo> Parameters = Ctor.GetParameters();
                                        ParamIndex = 0;
                                        while (ParamIndex < Parameters.Count())
                                        {
                                            Param = Parameters.ElementAt(ParamIndex);
                                            InnerAsm = Param.ParameterType.Assembly.GetName().Name + ".dll";
                                            if (LoadedAssemblyNames.Where(x => x.Key == InnerAsm).FirstOrDefault().Key == null &&
                                                InnerAsm != this.container.FrameWorkName + ".dll" &&
                                                InnerAsm.ToLower().IndexOf("microsoft.") == -1 &&
                                                InnerAsm.ToLower().IndexOf("system.") == -1)
                                            {
                                                LoadedAssemblyNames.Add(InnerAsm, 0);
                                            }
                                            ParamIndex++;
                                        }
                                        CtorIndex++;
                                    }
                                }
                                TypeIndex++;
                            }
                        }
                    }
                    LoadedAssemblyNames[LoadedAssemblyNames.ElementAt(Index).Key] = 1;
                    Index++;
                }
                Assemblies = null;
                return aPIManager;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "LoadAssemblies()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "LoadAssemblies()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }
    }
}

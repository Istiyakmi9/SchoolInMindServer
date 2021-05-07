using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.ContextFactoryManager.Code;
using BottomhalfCore.ContextFactoryManager.Interface;
using BottomhalfCore.Exceptions;
using BottomhalfCore.IFactoryContext;
using IFactoryContext.IFactoryContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BottomhalfCore.FactoryContext
{


    public class LoadAsmTypes : ILoadAsmTypes<LoadAsmTypes>
    {
        private List<Assembly> assemblyList = null;
        private IContainer container;
        private string Bindir = null;

        public LoadAsmTypes()
        {
            this.container = Container.GetInstance();
        }

        /// <summary>DiscoverClassFiles
        /// <para></para>
        /// </summary>
        public Dictionary<string, TypeRefCollection> DiscoverClassFiles()
        {
            List<AopDetail> aopDetailLst;
            IManageTypeCollection<ManageTypeCollection> manageTypeCollection;
            ILoasTypeDetail<LoadTypeDetail> loasTypeDetail;
            Dictionary<string, TypeRefCollection> ClassTypeCollection = null;
            Dictionary<string, List<InterfaceClassLinker>> interfaceClassLinker = null;
            try
            {
                aopDetailLst = null;
                //assemblyList = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
                assemblyList = LoadAssemblies();
                manageTypeCollection = new ManageTypeCollection();
                loasTypeDetail = new LoadTypeDetail(assemblyList);
                ClassTypeCollection = loasTypeDetail.BuildClassInformation(aopDetailLst);
                ICircularDependencyCheck _iCircularDependencyCheck = new CircularDependencyCheck(assemblyList);
                if (_iCircularDependencyCheck.LoadTypeForDependencyCheck() && ClassTypeCollection != null && ClassTypeCollection.Count() > 0)
                {
                    interfaceClassLinker = manageTypeCollection.MapInterfaceToClasses(ClassTypeCollection, aopDetailLst);
                    if (interfaceClassLinker != null)
                        container.SetInterfaceClassLinker(interfaceClassLinker);
                    manageTypeCollection.ManageConstructor(ClassTypeCollection);
                    if (container.MSSqlState)
                    {
                        TypeRefCollection ObjTypeRefCollection = manageTypeCollection.GetDbRefType();
                        ClassTypeCollection.Add("BottomhalfCore.DatabaseLayer.MsSql.Code.Db".ToLower(), ObjTypeRefCollection);
                        ClassTypeCollection.Add("BottomhalfCore.DatabaseLayer.Common.Code.IDb".ToLower(), ObjTypeRefCollection);
                    }
                    else if (container.MYSqlState)
                    {
                        TypeRefCollection ObjTypeRefCollection = manageTypeCollection.GetMySqlDbRefType();
                        ClassTypeCollection.Add("BottomhalfCore.DatabaseLayer.MySql.Code.Db".ToLower(), ObjTypeRefCollection);
                        ClassTypeCollection.Add("BottomhalfCore.DatabaseLayer.Common.Code.IDb".ToLower(), ObjTypeRefCollection);
                    }
                }
                return ClassTypeCollection;
            }
            catch (BeanException _beanEx)
            {
                ClassTypeCollection = null;
                interfaceClassLinker = null;
                assemblyList = null;
                _beanEx.LocationTrack(this.GetType().FullName + "DiscoverClassFiles()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "DiscoverClassFiles()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>LoadAssemblies
        /// <para></para>
        /// </summary>
        private List<Assembly> LoadAssemblies()
        {
            try
            {
                string Name = null;
                Assembly asm = null;
                List<Assembly> Assemblies = new List<Assembly>();
                Bindir = container.GetProjectBinPath();
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
                                    //ReadAttributeAndParameters(type);
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

                #region DEADCODE


                //foreach (var dir in Files)
                //{
                //    foreach (string dll in AsmFiles)
                //    {
                //        Name = dll.Substring(dll.LastIndexOf(@"\"), dll.Length - dll.LastIndexOf(@"\")).Replace(@"\", "").Replace(".dll", "");
                //        if (dir == Name && Name == dir && Name != this.container.FrameWorkName)
                //        {
                //            asm = null;
                //            if (Assemblies.Where(x => x.GetName().Name == Name).FirstOrDefault() == null)
                //            {
                //                asm = Assembly.LoadFrom(dll);
                //                Assemblies.Add(asm);
                //            }
                //            break;
                //        }
                //    }

                //    if (AsmExeFiles.Count() > 0)
                //    {
                //        foreach (string ExeFile in AsmExeFiles)
                //        {
                //            Name = ExeFile.Substring(ExeFile.LastIndexOf(@"\"), ExeFile.Length - ExeFile.LastIndexOf(@"\")).Replace(@"\", "").Replace(".exe", "");
                //            if (dir == Name && Name == dir && Name != this.container.FrameWorkName)
                //            {
                //                asm = null;
                //                if (Assemblies.Where(x => x.GetName().Name == Name).FirstOrDefault() == null)
                //                {
                //                    asm = Assembly.LoadFile(ExeFile);
                //                    Assemblies.Add(asm);
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}

                #endregion

                return Assemblies;
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

        #region RUNTIME LIB LOADER

        /// <summary>SetUpContextEnvironment
        /// <para></para>
        /// </summary>
        public Dictionary<string, TypeRefCollection> DiscoverDynamicLibrary(string LibName)
        {
            List<AopDetail> aopDetail = null;
            IManageTypeCollection<ManageTypeCollection> manageTypeCollection;
            ILoasTypeDetail<LoadTypeDetail> loasTypeDetail;
            Dictionary<string, TypeRefCollection> ClassTypeCollection = null;
            Dictionary<string, List<InterfaceClassLinker>> interfaceClassLinker = null;
            IAssemblyHandler _iAssemblyHandler = new AssemblyHandler();
            assemblyList = _iAssemblyHandler.LoadNamedAssemblies(LibName, ref Bindir);
            ICircularDependencyCheck _iCircularDependencyCheck = new CircularDependencyCheck(assemblyList);
            if (_iCircularDependencyCheck.LoadTypeForDependencyCheck())
            {
                manageTypeCollection = new ManageTypeCollection();
                loasTypeDetail = new LoadTypeDetail(assemblyList);
                ClassTypeCollection = loasTypeDetail.BuildClassInformation(aopDetail);
                interfaceClassLinker = manageTypeCollection.MapInterfaceToClasses(ClassTypeCollection, aopDetail);
                manageTypeCollection.ManageConstructor(ClassTypeCollection);
            }
            return ClassTypeCollection;
        }

        #endregion
    }
}

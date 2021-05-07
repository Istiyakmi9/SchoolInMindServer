using BottomhalfCore.Exceptions;
using BottomhalfCore.IFactoryContext;
using IFactoryContext.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BottomhalfCore.FactoryContext
{
    public class CircularDependencyCheck : ICircularDependencyCheck
    {
        List<string> WhiteList = null;
        IContainer container = null;
        List<Assembly> AssemblyList = null;
        private Boolean IsFullyGenericTypes = false;
        public CircularDependencyCheck(List<Assembly> AssemblyList)
        {
            WhiteList = new List<string>();
            container = Container.GetInstance();
            this.AssemblyList = AssemblyList;
        }
        public Boolean LoadTypeForDependencyCheck()
        {
            Assembly asm = null;
            int AssemblyIndex = 0;
            List<Type> ClassTypes = new List<Type>();
            Boolean ValidFlag = false;

            try
            {
                while (AssemblyIndex < AssemblyList.Count)
                {
                    asm = AssemblyList[AssemblyIndex];
                    Type[] TypeCollection = asm.GetTypes();
                    if (TypeCollection.Length > 0)
                    {
                        ClassTypes.AddRange(TypeCollection.ToList<Type>());
                    }
                    AssemblyIndex++;
                }

                List<string> GrayList = null;
                foreach (Type type in ClassTypes)
                {
                    GrayList = new List<string>();
                    CheckCircularDependent(type, GrayList, null);
                }
                ValidFlag = true;
                return ValidFlag;
            }
            catch (CircularReference crx)
            {
                throw crx;
            }
        }
        private void CheckCircularDependent(Type ObjectType, List<string> GrayList, List<string> CurrentTypes)
        {
            string WrapperName = null;
            if (ObjectType.Namespace != null && ObjectType.FullName.IndexOf("<") == -1 && ObjectType.FullName.IndexOf("<>") == -1)
            {
                if (ObjectType.IsGenericType)
                {
                    WrapperName = ObjectType.FullName;
                    if (ObjectType.GetInterfaces().Where(x => x.Name == "IEnumerable").Any())
                    {
                        foreach (var argument in ObjectType.GetGenericArguments())
                        {
                            if (!container.IsPrimitiveType(argument))
                            {
                                CheckCircularDependent(argument, GrayList, CurrentTypes);
                                if (!argument.GetInterfaces().Where(x => x.Name == "IEnumerable").Any())
                                    if (!CurrentTypes.Where(x => x == argument.FullName).Any())
                                        CurrentTypes.Add(argument.FullName);
                            }
                        }
                    }
                    else
                    {
                        foreach (var Args in ObjectType.GetGenericArguments())
                        {
                            if (Args.FullName != null)
                            {
                                if (!container.IsPrimitiveType(ObjectType))
                                {
                                    CheckCircularDependent(Args, GrayList, CurrentTypes);
                                    if (!Args.GetInterfaces().Where(x => x.Name == "IEnumerable").Any() && !Args.IsGenericType && Args.FullName != null)
                                        if (!CurrentTypes.Where(x => x == Args.FullName).Any())
                                            CurrentTypes.Add(Args.FullName);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ObjectType.FullName != null && !ObjectType.IsInterface)
                    {
                        if (!GrayList.Where(x => x == ObjectType.FullName).Any())
                        {
                            if (!WhiteList.Where(x => x == ObjectType.FullName).Any())
                            {
                                GrayList.Add(ObjectType.FullName);
                                ConstructorInfo[] Constructors = ObjectType.GetConstructors();
                                CurrentTypes = new List<string>(); ;
                                if (Constructors.Count() == 0)
                                    Constructors = ObjectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                foreach (ConstructorInfo constInfo in Constructors)
                                {
                                    if (constInfo.GetParameters().Count() > 0)
                                    {
                                        foreach (ParameterInfo parameters in constInfo.GetParameters())
                                        {
                                            if (!container.IsPrimitiveType(parameters.ParameterType))
                                            {
                                                CheckCircularDependent(parameters.ParameterType, GrayList, CurrentTypes);
                                                if (!parameters.ParameterType.GetInterfaces().Where(x => x.Name == "IEnumerable").Any() && !parameters.ParameterType.IsGenericType)
                                                    if (!CurrentTypes.Where(x => x == parameters.ParameterType.FullName).Any())
                                                    {
                                                        if (parameters.ParameterType.IsGenericType)
                                                        {
                                                            CurrentTypes.Add(parameters.ParameterType.FullName);
                                                            WrapperName = null;
                                                        }
                                                        CurrentTypes.Add(parameters.ParameterType.FullName);
                                                    }
                                            }
                                        }
                                    }

                                    GrayList.Remove(ObjectType.FullName);
                                    if (!WhiteList.Where(x => x == ObjectType.FullName).Any())
                                        WhiteList.Add(ObjectType.FullName);
                                }
                            }
                        }
                        else
                        {
                            throw new CircularReference(GrayList, ObjectType);
                        }
                    }
                    else
                    {
                        IsFullyGenericTypes = true;
                    }
                }
            }
        }
    }
}

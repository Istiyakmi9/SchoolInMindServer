using BottomhalfCore.Factory.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BottomhalfCore.Factory.FactoryContext
{
    public class TypeNObject
    {
        public Type InstanceType { set; get; }
        public object Instance { set; get; }
    }
    public class CloneObjectSchema : ICloneObjectSchema<CloneObjectSchema>
    {
        private object NewObject = null;
        public object GetEmptyObject<T>(T Instance)
        {
            Type InstanceType = typeof(T);
            NewObject = null;

            return NewObject;
        }

        public object GetEmptyObject(Type InstanceType)
        {
            Type type = null;
            (NewObject, type) = BuildEmptyObject(InstanceType);
            return NewObject;
        }

        private (object, Type) BuildEmptyObject(Type InstanceType)
        {
            NewObject = null;
            Type ArgType = null;
            List<TypeNObject> ArgTypes = null;
            try
            {
                string AssemblyName = InstanceType.AssemblyQualifiedName;
                // ----------- Given type is collection type
                if (InstanceType.IsGenericType)
                {
                    ArgTypes = new List<TypeNObject>();
                    Type[] types = InstanceType.GenericTypeArguments;
                    int TypeIndex = 0;
                    while (TypeIndex < types.Length)
                    {
                        (NewObject, ArgType) = BuildEmptyObject(types.ElementAt(TypeIndex));
                        ArgTypes.Add(new TypeNObject { InstanceType = ArgType, Instance = NewObject });
                        TypeIndex++;
                    }

                    if (InstanceType.GetInterfaces().Where(x => x.Name == "IEnumerable").FirstOrDefault() != null)
                    {
                        if (InstanceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) ||
                            InstanceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IList<>)))
                        {
                            object CollectionObject = GetList(ArgTypes.ElementAt(0).InstanceType);
                            InstanceType.GetMethod("Insert").Invoke(CollectionObject, new[] { 0, NewObject });
                            ArgType = InstanceType;
                            NewObject = CollectionObject;
                        }
                        else if (InstanceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)) ||
                            InstanceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IDictionary<,>)))
                        {
                            object CollectionObject = GetDictonary(ArgTypes.ElementAt(0).InstanceType, ArgTypes.ElementAt(1).InstanceType);
                            InstanceType.GetMethod("Add").Invoke(CollectionObject,
                                new[] { ArgTypes.ElementAt(0).Instance, ArgTypes.ElementAt(1).Instance });
                            ArgType = InstanceType;
                            NewObject = CollectionObject;
                        }
                    }
                    else
                    {
                        (NewObject, ArgType) = CreateAndInitializeObject(InstanceType);
                    }
                }
                else
                {
                    //if (AssemblyName.IndexOf("System.") != -1)
                    //{
                    //    (NewObject, ArgType) = CreateAndInitializeObject(InstanceType);
                    //}
                    //else
                    //{
                    (NewObject, ArgType) = CreateAndInitializeObject(InstanceType);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (NewObject, ArgType);
        }

        private (object, Type) CreateAndInitializeObject(Type InstanceType)
        {
            Type CurrentType = null;
            object CurrentInstance = null;
            PropertyInfo[] propertyInfos = InstanceType.GetProperties();
            if (InstanceType == typeof(string))
            {
                CurrentInstance = "";
                CurrentType = typeof(string);
            }
            else
            {
                object PropObject = null;
                CurrentInstance = Activator.CreateInstance(InstanceType);
                int i = 0;
                while (i < propertyInfos.Length)
                {
                    CurrentType = propertyInfos.ElementAt(i).PropertyType;
                    if (propertyInfos.ElementAt(i).PropertyType.IsValueType)
                        PropObject = Activator.CreateInstance(CurrentType);
                    else
                    {
                        if (CurrentType == typeof(string))
                        {
                            PropObject = "";
                            CurrentType = typeof(string);
                        }
                        else
                        {
                            (PropObject, CurrentType) = BuildEmptyObject(CurrentType);
                        }
                    }
                    propertyInfos.ElementAt(i).SetValue(CurrentInstance, PropObject);
                    i++;
                }
            }

            return (CurrentInstance, InstanceType);
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
    }
}

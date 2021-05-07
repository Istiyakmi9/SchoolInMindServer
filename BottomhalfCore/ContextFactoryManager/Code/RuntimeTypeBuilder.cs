using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Code
{
    public class Fields
    {
        public Fields(string fieldName, Type fieldType)
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }
        public string FieldName { get; }
        public Type FieldType { get; }
    }

    public static class RuntimeTypeBuilder<FieldDescriptor>
    {
        public static (Type, string) CreateRuntimeType(List<Fields> fields, string TypeName)
        {
            Type newType = null;
            string Message = default(string);
            if (fields != null && fields.Count() > 0)
            {
                TypeInfo typeInfo = CompileResultTypeInfo(fields, TypeName);
                newType = typeInfo.AsType();
                Message = "Created Successfully";
            }
            else
            {
                Message = "List fo fields value is empty or null";
            }
            return (newType, Message);
        }

        public static (object, string) CreateRuntimeInstance(List<Fields> fields, string TypeName)
        {
            string Message = default(string);
            Type newType = default(Type);
            (newType, Message) = CreateRuntimeType(fields, TypeName);
            object newObject = Activator.CreateInstance(newType);
            return (newObject, Message);
        }

        public static TypeInfo CompileResultTypeInfo(List<Fields> fields, string TypeName)
        {
            TypeBuilder tb = GetTypeBuilder(TypeName);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            int Index = 0;
            while (Index < fields.Count())
            {
                CreateProperty(tb, fields.ElementAt(Index).FieldName, fields.ElementAt(Index).FieldType);
                Index++;
            }

            TypeInfo objectTypeInfo = tb.CreateTypeInfo();
            return objectTypeInfo;
        }

        private static TypeBuilder GetTypeBuilder(string TypeName)
        {
            var typeSignature = TypeName;
            var an = new AssemblyName(typeSignature);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}

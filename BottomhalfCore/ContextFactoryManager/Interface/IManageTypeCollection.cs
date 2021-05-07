using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Interface
{
    public interface IManageTypeCollection<T>
    {
        /// <summary>GetDbRefType for MsSql type
        /// <para></para>
        /// </summary>
        TypeRefCollection GetDbRefType();

        /// <summary>GetDbRefType for Mysql
        /// <para></para>
        /// </summary>
        TypeRefCollection GetMySqlDbRefType();

        /// <summary>MapInterfaceToClasses
        /// <para></para>
        /// </summary>
        Dictionary<string, List<InterfaceClassLinker>> MapInterfaceToClasses(Dictionary<string, TypeRefCollection> TypeCollection, List<AopDetail> aopDetailLst);

        /// <summary>ManageConstructor
        /// <para></para>
        /// </summary>
        void ManageConstructor(Dictionary<string, TypeRefCollection> ClassTypeCollection);

        /// <summary>GetImplementedName
        /// <para></para>
        /// </summary>
        string GetImplementedName(string GenericName);

        /// <summary>ConvertNameSpaceToList
        /// <para></para>
        /// </summary>
        List<string> ConvertNameSpaceToList(string SplittedName);
    }
}

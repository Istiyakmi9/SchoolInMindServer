using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Interface
{
    public interface IManageAopDetail<T>
    {
        void FindAopOnClass(TypeRefCollection ClassDetail, List<AopDetail> aopDetailLst);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BottomhalfCore.Factory.IFactoryContext
{
    public interface ICloneObjectSchema<T>
    {
        object GetEmptyObject<K>(K Instance);
        object GetEmptyObject(Type Instance);
    }
}

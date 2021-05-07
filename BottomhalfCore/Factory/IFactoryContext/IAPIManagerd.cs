using BottomhalfCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomhalfCore.Factory.IFactoryContext
{
    public interface IAPIManagerd<T>
    {
        Dictionary<string, List<APIManagerModal>> GetAPIs();
    }
}

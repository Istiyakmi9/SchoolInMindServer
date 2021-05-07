using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.CacheManagement
{
    public interface IAccessor<T>
    {
        string KeyName { set; get; }
        Object LoadData();
    }
}

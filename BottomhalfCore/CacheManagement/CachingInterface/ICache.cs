using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.CacheManagement.CachingInterface
{
    public interface ICache<T>
    {
        bool Put(string key, object value);
        void Append(string Key, Object Value);
        Boolean Replace(string Key, Object Value);
        object Get(string key);
        bool ContainsKey(string key);
        Type GetIType(string key);
        Boolean ReSet();
    }
}

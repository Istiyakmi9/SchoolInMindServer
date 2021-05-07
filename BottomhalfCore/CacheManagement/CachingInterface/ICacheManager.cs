using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.CacheManagement.CachingInterface
{
    public interface ICacheManager<T>
    {
        void Put(string Key, Object Value);
        Boolean ContainersKey(string key);
        Object Get(string Key);
        void Append(string Key, Object Value);
        Boolean ReplaceContainer(string Key, Object Value);
        Boolean ReSet();
        void LoadData();
        void Add(string Uid, Object CurrentObject);
        bool Remove(string Uid);
    }
}

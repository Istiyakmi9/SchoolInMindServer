using BottomhalfCore.CacheManagement.Caching;
using BottomhalfCore.CacheManagement.CachingInterface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.CacheManagement
{
    public class Cache : ICache<Cache>
    {
        private static ICache<CacheManager> instance = null;
        private readonly static object _lock = new object();
        private IDictionary<string, object> map = null;
        public Cache()
        {
            map = new ConcurrentDictionary<string, object>();
        }

        public bool Put(string key, object value)
        {
            bool addState = false;
            Object RemovedValue = null;
            ((ConcurrentDictionary<string, object>)map).TryRemove(key, out RemovedValue);
            if (RemovedValue == null)
                addState = ((ConcurrentDictionary<string, object>)map).TryAdd(key, value);
            return addState;
        }

        public void Append(string Key, Object Value)
        {
            object RetrieveValue = null;
            ((ConcurrentDictionary<string, object>)map).TryGetValue(Key, out RetrieveValue);
            if (RetrieveValue != null)
            {
                List<string> ObjRecord = null;
                ObjRecord = RetrieveValue as List<string>;
                ObjRecord.Add(Value.ToString());
                map[Key] = ObjRecord;
            }
            else
            {
                List<string> ObjRecord = new List<string>();
                ObjRecord.Add(Value.ToString());
                ((ConcurrentDictionary<string, object>)map).TryAdd(Key, ObjRecord);
            }
        }

        public Boolean Replace(string Key, Object Value)
        {
            Boolean SFlag = false;
            object RetrieveValue = null;
            ((ConcurrentDictionary<string, object>)map).TryGetValue(Key, out RetrieveValue);
            if (RetrieveValue == null)
            {
                map.Add(Key, Value);
                SFlag = true;
            }
            else
            {
                map[Key] = Value; ;
                SFlag = true;
            }
            return SFlag;
        }

        public object Get(string key)
        {
            object retrieveValue = null;
            ((ConcurrentDictionary<string, object>)map).TryGetValue(key, out retrieveValue);
            return retrieveValue;
        }

        public Boolean ReSet()
        {
            Boolean Flag = false;
            map = new ConcurrentDictionary<string, Object>();
            return Flag;
        }

        public bool ContainsKey(string key)
        {
            return map.ContainsKey(key);
        }

        public Type GetIType(string key)
        {
            Type requiredType = null;
            var allType = instance.Get("iType");
            IDictionary<string, Type> type = (IDictionary<string, Type>)allType;
            type.TryGetValue(key, out requiredType);
            return requiredType;
        }
    }
}

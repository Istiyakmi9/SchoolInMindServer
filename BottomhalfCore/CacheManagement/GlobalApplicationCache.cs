using BottomhalfCore.CacheManagement.CachingInterface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BottomhalfCore
{
    public class GlobalApplicationCache<T> where T : ICacheManager<T>
    {
        private static GlobalApplicationCache<T> instance = null;
        private readonly static object _lock = new object();
        private IDictionary<string, object> map = null;
        private GlobalApplicationCache()
        {
            map = new ConcurrentDictionary<string, object>();
        }

        public static GlobalApplicationCache<T> GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new GlobalApplicationCache<T>();
                }
            }

            return instance;
        }

        public bool put(string key, object value)
        {
            bool addState = ((ConcurrentDictionary<string, object>)map).TryAdd(key, value);
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

        public Boolean replace(string Key, Object Value)
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

        public object get(string key)
        {
            object retrieveValue = null;
            ((ConcurrentDictionary<string, object>)map).TryGetValue(key, out retrieveValue);
            return retrieveValue;
        }

        public bool ContainsKey(string key)
        {
            return map.ContainsKey(key);
        }

        public Type GetIType(string key)
        {
            Type requiredType = null;
            var allType = instance.get("iType");
            IDictionary<string, Type> type = (IDictionary<string, Type>)allType;
            type.TryGetValue(key, out requiredType);
            return requiredType;
        }
    }
}

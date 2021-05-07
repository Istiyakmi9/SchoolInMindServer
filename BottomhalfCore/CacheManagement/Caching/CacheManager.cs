using BottomhalfCore.CacheManagement.CachingInterface;
using BottomhalfCore.FactoryContext;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.CacheManagement.Caching
{
    public class CacheManager : ICacheManager<CacheManager>
    {
        private static CacheManager CacheManagerInstance = null;
        private ConcurrentDictionary<string, dynamic> ScopedContainer;
        public static readonly Object _lock = new object();
        private ICache<Cache> cache;
        private CacheManager()
        {
            cache = new Cache();
            ScopedContainer = new ConcurrentDictionary<string, Object>();
        }

        public void Add(string Uid, Object CurrentObject)
        {
            ScopedContainer.TryAdd(Uid, CurrentObject);
        }

        public bool Remove(string Uid)
        {
            return ScopedContainer.TryRemove(Uid, out Object RemovedObject);
        }

        public static CacheManager GetInstance()
        {
            if (CacheManagerInstance == null)
            {
                lock (_lock)
                {
                    if (CacheManagerInstance == null)
                        CacheManagerInstance = new CacheManager();
                }
            }

            return CacheManagerInstance;
        }

        public Boolean ReplaceContainer(string Key, Object Value)
        {
            return false;
        }

        public Type GetGenericType(string key)
        {
            return null;
        }

        public void LoadData()
        {
            BeanContext context = BeanContext.GetInstance();
            Object ObjectInstance = null;
            Object Data = null;
            List<Type> Accessors = context.GetAccessorTypes();
            foreach (Type Accessor in Accessors)
            {
                if (!Accessor.IsInterface)
                {
                    ObjectInstance = null;
                    Data = null;
                    MethodInfo Method = Accessor.GetMethod("LoadData");
                    ObjectInstance = context.GetBean(Accessor);
                    Data = Method.Invoke(ObjectInstance, null);
                    if (Data != null)
                    {
                        PropertyInfo propertyInfo = Accessor.GetProperty("KeyName");
                        Object KeyName = propertyInfo.GetValue(ObjectInstance);
                        if (KeyName != null)
                            this.Put(KeyName.ToString(), Data);
                    }
                }
            }
        }

        public Boolean ReSet()
        {
            return false;
        }

        public void Append(string Key, Object Value)
        {

        }

        #region CONTAINER FUNCTION CALLS

        public void Put(string Key, Object Value)
        {
            cache.Put(Key.ToLower(), Value);
        }

        public Object Get(string Key)
        {
            return cache.Get(Key.ToLower());
        }

        public Boolean ContainersKey(string key)
        {
            return false;
        }

        #endregion
    }
}

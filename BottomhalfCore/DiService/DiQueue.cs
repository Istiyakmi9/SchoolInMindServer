using BottomhalfCore.FactoryContext;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BottomhalfCore.DiService
{
    public class DiQueue
    {
        public static DiQueue diQueue;
        public static object _lock = new object();
        private readonly BeanContext context;
        private DiQueue()
        {
            context = BeanContext.GetInstance();
            ScopeObjectList = new ConcurrentDictionary<string, List<InjectionObjects>>();
            SingletonObjectList = new List<InjectionObjects>();
            ScopedClassList = new List<string>();
        }

        public static DiQueue GetInstance()
        {
            if (diQueue == null)
            {
                lock (_lock)
                {
                    if (diQueue == null)
                        diQueue = new DiQueue();
                }
            }
            return diQueue;
        }

        public ConcurrentDictionary<string, List<InjectionObjects>> ScopeObjectList { set; get; }
        public List<InjectionObjects> SingletonObjectList { set; get; }
        public List<string> ScopedClassList { set; get; }

        public void AddScope<T>()
        {
            Type type = typeof(T);
            if (!ScopedClassList.Contains(type.FullName))
                ScopedClassList.Add(type.FullName);
        }

        public void InitScopeQueue(string UniqueKey)
        {
            Parallel.ForEach(ScopedClassList, ClassName =>
            {
                Object NewObject = context.GetBean(ClassName, null);
                if (NewObject != null)
                {
                    if (!ScopeObjectList.ContainsKey(UniqueKey))
                        ScopeObjectList.TryAdd(UniqueKey, new List<InjectionObjects>());
                    var CurrentSesstionObjectList = ScopeObjectList.Where(x => x.Key == UniqueKey).FirstOrDefault().Value;
                    if (CurrentSesstionObjectList != null)
                    {
                        CurrentSesstionObjectList.Add(new InjectionObjects { QualifiedName = ClassName, ClassObject = NewObject });
                        ScopeObjectList.TryAdd(UniqueKey, CurrentSesstionObjectList);
                    }
                }
            });
        }

        public bool RemoveScoped(string UniqueId)
        {
            bool StatusFlag = false;
            if (ScopeObjectList.ContainsKey(UniqueId))
                StatusFlag = ScopeObjectList.TryRemove(UniqueId, out List<InjectionObjects> value);
            return StatusFlag;
        }

        public Object GetScoped(Type ObjectType, string UniqueId)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Object RequestedObject = null;
            Task SearchObjectTask = Task.Run(() =>
            {
                var CurrentScopeList = ScopeObjectList.Where(x => x.Key == UniqueId).FirstOrDefault().Value;
                if (CurrentScopeList != null)
                {
                    Parallel.ForEach(CurrentScopeList, (x, state) =>
                    {
                        if (x.QualifiedName == ObjectType.FullName)
                        {
                            RequestedObject = x.ClassObject;
                            tokenSource.Cancel();
                            state.Break();
                        }
                    });
                }
            }, token);

            token.Register(() =>
            {
                if (RequestedObject == null)
                {

                }
            });
            Task.WaitAll(SearchObjectTask);
            return RequestedObject;
        }

        public T GetScoped<T>(string UniqueId)
        {
            Type type = typeof(T);
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Object RequestedObject = null;
            Task SearchObjectTask = Task.Run(() =>
            {
                var CurrentScopeList = ScopeObjectList.Where(x => x.Key == UniqueId).FirstOrDefault().Value;
                if (CurrentScopeList != null)
                {
                    Parallel.ForEach(CurrentScopeList, (x, state) =>
                    {
                        if (x.QualifiedName == type.FullName)
                        {
                            RequestedObject = x.ClassObject;
                            tokenSource.Cancel();
                            state.Break();
                        }
                    });
                }
            }, token);

            token.Register(() =>
            {
                if (RequestedObject == null)
                {

                }
            });
            Task.WaitAll(SearchObjectTask);
            return (T)RequestedObject;
        }

        public void AddToSingletonQueue(string QualifiedName)
        {
        }
    }

    public class InjectionObjects
    {
        public string QualifiedName { set; get; }
        public Object ClassObject { set; get; }
    }
}

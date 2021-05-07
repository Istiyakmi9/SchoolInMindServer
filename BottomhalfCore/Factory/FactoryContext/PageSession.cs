using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.Exceptions;
using BottomhalfCore.Flags;
using BottomhalfCore.IFactoryContext;
using System;
using System.Collections.Concurrent;

namespace BottomhalfCore.FactoryContext
{
    public class PageSession : IPageSession<PageSession>
    {
        ConcurrentDictionary<string, SessionObject> ObjSesstionCache = null;
        private SessionObject ObjExistingSessionObject = null;
        private static readonly object _lock = new object();
        private static PageSession instance = null;
        BeanContext context;
        private string LandingPageUrl { set; get; }
        private PageSession()
        {
            ObjSesstionCache = new ConcurrentDictionary<string, SessionObject>();
        }

        private BeanContext GetAnnotationContextObject()
        {
            if (context == null)
                context = BeanContext.GetInstance();
            return context;
        }

        public static PageSession GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new PageSession();
                    }
                }
            }
            return instance;
        }

        public Boolean CleanUp()
        {
            Boolean Flag = false;
            // Perform some clean up activity
            return Flag;
        }

        public string GetDefaultUrl()
        {
            string Url = this.LandingPageUrl;
            return Url;
        }

        public static void ValidateToken(string Key)
        {
            Boolean Flag = instance.ValidateCurrentRequestToken(Key);
            if (!Flag)
                instance.FireException("Unable to resolve token", EFlags.InvalidToken, Key);
        }

        public void SetClientCookies(string Token)
        {
            //HttpResponse ResponseContext = System.Web.HttpContext.Current.Response;
            //HttpCookie ObjHttpCookie = ResponseContext.Cookies["x-request-token"];
            //if (Token != null)
            //{
            //    if (ObjHttpCookie != null)
            //    {
            //        ResponseContext.Cookies.Remove("x-request-token");
            //        ObjHttpCookie.Expires = DateTime.Now.AddSeconds(0);
            //        ObjHttpCookie.Value = null;
            //    }
            //    ObjHttpCookie = new HttpCookie("x-request-token", Token);
            //    ObjHttpCookie.Expires = DateTime.Now.AddMinutes(20);
            //}
            //else
            //{
            //    ResponseContext.Cookies.Remove("x-request-token");
            //    ObjHttpCookie.Expires = DateTime.Now.AddSeconds(-1);
            //    ObjHttpCookie.Value = null;
            //}
            //ResponseContext.Cookies.Add(ObjHttpCookie);
        }

        public Boolean ValidateCurrentRequestToken(string Key)
        {
            Boolean Flag = false;
            ObjExistingSessionObject = null;
            ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
            if (ObjExistingSessionObject != null)
            {
                Double TotalSeconds = (DateTime.Now - ObjExistingSessionObject.LastUpdatedOn).TotalSeconds;
                if (TotalSeconds > 1200)
                {
                    SetClientCookies(null);
                    ObjSesstionCache.TryRemove(Key, out ObjExistingSessionObject);
                    FireException("Token expired.", EFlags.TokenExpired, Key);
                }
                else
                {
                    ObjExistingSessionObject.LastUpdatedOn = DateTime.Now;
                    //ObjSesstionCache[Key] = ObjExistingSessionObject;
                    SetClientCookies(Key);
                    Flag = true;
                }

            }
            else
            {
                FireException("Token not found.", EFlags.TokenNotFound, Key);
            }
            return Flag;
        }

        public static String Add(string key, Object UserObject, string ConnectionString)
        {
            String GeneratedToken = null;
            DateTime TimeStamp = DateTime.Now;
            instance.context = instance.GetAnnotationContextObject();
            GeneratedToken = instance.context.ComputeSha256Hash(key + TimeStamp.ToString());
            if (!instance.AddSession(GeneratedToken, UserObject, ConnectionString))
                GeneratedToken = null;
            return GeneratedToken;
        }

        public static String Update(string Key, Object UserObject, string ConnectionString)
        {
            String GeneratedToken = null;
            DateTime TimeStamp = DateTime.Now;
            instance.context = instance.GetAnnotationContextObject();
            if (!instance.AddSession(Key, UserObject, ConnectionString))
                GeneratedToken = "Success";
            return GeneratedToken;
        }

        private void FireException(string Message, EFlags ExceptionCode, string Token)
        {
            SessionException exception = new SessionException();
            exception.SetMessage(Message);
            exception.Token = Token;
            exception.ExceptionCode = ExceptionCode;

            string Url = GetDefaultUrl();
            if (Url != null)
            {
                throw exception;
            }
            else
            {
                throw new ApplicationException("Session expire. Please login again.");
            }
        }

        public static Boolean Remove(string Key)
        {
            Boolean Flag = false;
            SessionObject RemovedSessionObject = null;
            Flag = instance.ObjSesstionCache.TryRemove(Key, out RemovedSessionObject);
            return Flag;
        }

        public Object Get(string Key, out Boolean IsAvailable)
        {
            Object userDetail = null;
            ObjExistingSessionObject = null;
            IsAvailable = false;
            if (Key != null)
            {
                ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
                if (ObjExistingSessionObject != null)
                {
                    userDetail = ObjExistingSessionObject.UserObject;
                    IsAvailable = true;
                }
            }
            return userDetail;
        }

        public Object Get(string Token, string Key)
        {
            Object userDetail = null;
            ObjExistingSessionObject = null;
            if (Key != null)
            {
                ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
                if (ObjExistingSessionObject != null)
                    userDetail = ObjExistingSessionObject.UserObject;
            }
            return userDetail;
        }

        public SessionObject GetObject(string Key)
        {
            SessionObject UserDetailObject = null;
            ObjExistingSessionObject = null;
            if (Key != null)
            {
                ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
                if (ObjExistingSessionObject != null)
                    UserDetailObject = ObjExistingSessionObject;
                else
                    UserDetailObject = null;
            }
            return UserDetailObject;
        }

        private Boolean AddSession(string Key, Object UserObject)
        {
            Boolean Flag = false;
            ObjExistingSessionObject = null;
            ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
            if (ObjExistingSessionObject != null)
            {
                ObjExistingSessionObject.UserObject = UserObject;
                ObjExistingSessionObject.LastUpdatedOn = DateTime.Now;
                ObjSesstionCache[Key] = ObjExistingSessionObject;
            }
            else
            {
                SessionObject ObjUserObject = new SessionObject();
                ObjUserObject.UserObject = UserObject;
                ObjUserObject.LastUpdatedOn = DateTime.Now;
                ObjSesstionCache.TryAdd(Key, ObjUserObject);
                Flag = true;
            }
            return Flag;
        }

        private Boolean AddSession(string Key, Object UserObject, string CurrentSessionConnectionString)
        {
            Boolean Flag = false;
            ObjExistingSessionObject = null;
            ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
            if (ObjExistingSessionObject != null)
            {
                if (UserObject != null)
                    ObjExistingSessionObject.UserObject = UserObject;
                if (!string.IsNullOrEmpty(CurrentSessionConnectionString))
                    ObjExistingSessionObject.SessionConnectionString = CurrentSessionConnectionString;
                ObjExistingSessionObject.LastUpdatedOn = DateTime.Now;
                ObjSesstionCache[Key] = ObjExistingSessionObject;
            }
            else
            {
                SessionObject ObjUserObject = new SessionObject();
                ObjUserObject.UserObject = UserObject;
                ObjUserObject.LastUpdatedOn = DateTime.Now;
                ObjUserObject.SessionConnectionString = CurrentSessionConnectionString;
                ObjSesstionCache.TryAdd(Key, ObjUserObject);
                Flag = true;
            }
            return Flag;
        }

        public Boolean AddSessionConnectionString(string Key, string ConnectionString)
        {
            Boolean Flag = false;
            ObjExistingSessionObject = null;
            ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
            if (ObjExistingSessionObject != null)
            {
                ObjExistingSessionObject.SessionConnectionString = ConnectionString;
                ObjSesstionCache[Key] = ObjExistingSessionObject;
            }
            else
            {
                throw new ApplicationException("Token not found");
            }
            return Flag;
        }

        public string GetConnectionString(string Key)
        {
            string ConnectionString = null;
            ObjExistingSessionObject = null;
            ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryGetValue(Key, out ObjExistingSessionObject);
            if (ObjExistingSessionObject != null)
            {
                ConnectionString = ObjExistingSessionObject.SessionConnectionString;
            }
            return ConnectionString;
        }

        public void Logout(string CurrentSession)
        {
            ObjExistingSessionObject = null;
            if (CurrentSession != "" && CurrentSession != null)
            {
                ((ConcurrentDictionary<string, SessionObject>)ObjSesstionCache).TryRemove(CurrentSession, out ObjExistingSessionObject);
                if (ObjExistingSessionObject != null)
                {
                    // Log user detail
                }
            }
        }
    }
}

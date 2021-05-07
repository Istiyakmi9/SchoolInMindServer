using BottomhalfCore.FactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.IFactoryContext
{
    public interface IPageSession<T> where T : class
    {
        string GetDefaultUrl();
        Boolean CleanUp();
        Boolean AddSessionConnectionString(string Key, string ConnectionString);
        string GetConnectionString(string Key);
        void SetClientCookies(string Token);
        void Logout(string Token);
        Object Get(string Key, out Boolean IsAvailable);
        Object Get(string Token, string Key);
    }
}
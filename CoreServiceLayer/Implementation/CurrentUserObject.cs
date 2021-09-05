using AuthenticationToken.Model;
using BottomhalfCore.CacheManagement.Caching;
using BottomhalfCore.FactoryContext;
using BottomhalfCore.IFactoryContext;
using CommonModal.ProcedureModel;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public abstract class CurrentUserObject : IServiceKeyIdentifier
    {
        public string CurrentKey { set; get; }
        public SessionModal userDetail;
        public string HeaderDetail;
        public DataSet ResultSet = null;
        public readonly BeanContext beanContext;

        public CurrentUserObject()
        {
            beanContext = BeanContext.GetInstance();
        }
    }
}

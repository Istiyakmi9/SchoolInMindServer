using IFactoryContext.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.FactoryContext
{
    public class UserCacheData : IUserCacheData
    {
        public UserCacheData()
        {
            ObjDataByKey = new List<DataByKey>();
        }
        public DateTime LastUpdatedOn { set; get; }
        public List<DataByKey> ObjDataByKey { set; get; }
        public string SessionConnectionString { set; get; }
    }

    public class DataByKey
    {
        public string Key { set; get; }
        public Object Value { set; get; }
    }
}

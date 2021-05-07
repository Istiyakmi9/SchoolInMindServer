using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFactoryContext.IFactoryContext
{
    public interface IUserCacheData
    {
    }

    public class DataByKey
    {
        public string Key { set; get; }
        public Object Value { set; get; }
    }
}

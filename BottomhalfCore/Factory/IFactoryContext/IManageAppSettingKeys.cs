using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.IFactoryContext
{
    public interface IManageAppSettingKeys<T>
    {
        void AddKey(IDictionary<string, string> KeyValuePair);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IMapper<T>
    {
        dynamic ConvertToObject(DataTable table, Type ExpectedObjectType, out string OperationMessage);
        dynamic ConvertListToObject(DataTable table, Type ExpectedObjectType, out string OperationMessage);
    }
}

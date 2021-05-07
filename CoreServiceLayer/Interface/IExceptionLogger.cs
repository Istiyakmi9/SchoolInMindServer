using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IExceptionLogger<T>
    {
        bool LogException(string Token, Boolean IsException, string ExceptionMessage, Boolean IsDestroyed);
    }
}

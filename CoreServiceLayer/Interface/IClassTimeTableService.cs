using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IClassTimeTableService<T>
    {
        string GetTimeTable(string StudentUid, string SchoolTenentId, string day);
    }
}

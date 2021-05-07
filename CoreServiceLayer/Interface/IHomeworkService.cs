using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IHomeworkService<T>
    {
        string StudHomeWork(string StudentUid, string SchoolTenentId);
    }

    public class HomeWorks
    {
        public string Title { set; get; }
        public string Teacher { set; get; }
        public string Description { set; get; }
        public string src { set; get; }
    }
}

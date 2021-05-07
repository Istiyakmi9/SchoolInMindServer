using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using ServiceLayer.Interface;

namespace CoreServiceLayer.Implementation
{
    public class HomeworkService : CurrentUserObject, IHomeworkService<HomeworkService>
    {
        public string ConnectionString = null;
        public void InitContainer()
        {
            this.ConnectionString = null;// context.GetWebConfigValue("DbCS");
        }
        public string StudHomeWork(string StudentUid, string SchoolTenentId)
        {
            return null;
        }
    }
}

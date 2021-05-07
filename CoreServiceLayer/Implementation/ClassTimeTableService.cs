using BottomhalfCore.DatabaseLayer.Common.Code;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class ClassTimeTableService : CurrentUserObject, IClassTimeTableService<ClassTimeTableService>
    {
        public string ConnectionString = null;
        private readonly IDb db;

        public string GetTimeTable(string StudentUid, string SchoolTenentId, string day)
        {
            string AuthedUserOBJ = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(StudentUid, typeof(System.String), "_studentId"),
                new DbParam(SchoolTenentId, typeof(System.String), "_schooltenentId"),
                new DbParam(day, typeof(System.String), "_day"),
            };

            DataSet ds = db.GetDataset("sp_ClassesTimetable_ByStudentId", param);
            if (ds != null && ds.Tables.Count > 0)
                AuthedUserOBJ = JsonConvert.SerializeObject(ds);
            return AuthedUserOBJ;
        }
    }
}

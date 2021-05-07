using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class FacultyDetailService : CurrentUserObject, IFacultyDetailService
    {
        private readonly IDb db;

        public FacultyDetailService(IDb db)
        {
            this.db = db;
        }
        public string GetFacultiesByStudentIdService(string StudentId, string SchooltenentId)
        {
            string AuthedUserOBJ = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(StudentId, typeof(System.String), "_studentId"),
                new DbParam(SchooltenentId, typeof(System.String), "_schooltenentId"),
            };

            DataSet ds = db.GetDataset("sp_GetFacultyDetail_ByStudentId", param);
            if (ds != null && ds.Tables.Count > 0)
                AuthedUserOBJ = JsonConvert.SerializeObject(ds);
            return AuthedUserOBJ;
        }
    }
}

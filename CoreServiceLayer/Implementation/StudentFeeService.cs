using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class StudentFeeService : CurrentUserObject, IStudentFeeService
    {
        private readonly IDb db;

        public string GetAcademicFeesInfo(CommonRequestObject objCommonRequestObject)
        {
            string resultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(objCommonRequestObject.StudentUid, typeof(System.String), "_studentUid"),
                new DbParam(2018, typeof(System.String), "_academicYearFrom"),
                new DbParam(objCommonRequestObject.SchoolTenentId, typeof(System.String), "_schooltenentId"),
            };
            DataSet ds = db.GetDataset("sp_GetFees_Detail", param);
            if (ds != null && ds.Tables.Count > 0)
                resultSet = JsonConvert.SerializeObject(ds);

            return resultSet;
        }
    }
}

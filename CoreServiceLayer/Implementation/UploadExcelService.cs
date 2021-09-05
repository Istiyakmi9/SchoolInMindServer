using BottomhalfCore.DatabaseLayer.Common.Code;
using ServiceLayer.Interface;

namespace CoreServiceLayer.Implementation
{
    public class UploadExcelService : CurrentUserObject, IUploadExcelService<UploadExcelService>
    {
        private readonly IDb db;
        string Result = string.Empty;
        public string UploadClassDetail(UploadXml objClassdetail, string ProcedureName)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(objClassdetail.xmlData, typeof(System.String), "_xmlData"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_schoolTenentId")
            };

            Result = db.ExecuteNonQuery(ProcedureName, param, true);
            return Result;
        }

        public string UploadStudentAttendenceDetail(UploadXml objClassdetail, string ProcedureName)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(objClassdetail.xmlData, typeof(System.String), "_xmlData"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_schoolTenentId")
            };

            Result = db.ExecuteNonQuery(ProcedureName, param, true);
            return Result;
        }

        public string UploadFacultyDetail(UploadXml objClassdetail, string ProcedureName)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(objClassdetail.xmlData, typeof(System.String), "_xmlData"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_schoolTenentId"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
            };

            Result = db.ExecuteNonQuery(ProcedureName, param, true);
            return Result;
        }

        public string UploadStudentDetailService(UploadXml objClassdetail, string ProcedureName)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(objClassdetail.xmlData, typeof(System.String), "_xmlData"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_schoolTenentId"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminId")
            };

            Result = db.ExecuteNonQuery(ProcedureName, param, true);
            return Result;
        }

        public string UploadAttendence()
        {
            return Result;
        }
        public string UploadResult()
        {
            return Result;
        }
        public string UploadDriverVehicle()
        {
            return Result;
        }
        public string UploadStudentDetail()
        {
            return Result;
        }
        public string UploadSubjects()
        {
            return Result;
        }
    }
}
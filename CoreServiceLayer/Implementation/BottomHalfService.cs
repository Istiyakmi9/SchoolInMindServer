using BottomhalfCore.DatabaseLayer.Common.Code;
using ServiceLayer.Interface;
using System;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class BottomHalfService : CurrentUserObject, IBottomHalfService<BottomHalfService>
    {
        private readonly IExceptionLogger<ExceptionLogger> exceptionLogger;
        private readonly IDb db;
        public BottomHalfService(IDb db, ExceptionLogger exceptionLogger)
        {
            this.db = db;
            this.exceptionLogger = exceptionLogger;
        }
        public string GetAccountByMobile(string UserId)
        {
            string Mobile = null;
            string Email = null;
            string Token = null;
            string ConnectionString = null;
            string UserFullName = null;
            string SchoolName = null;
            if (UserId.IndexOf(@"@") != -1)
                Email = UserId;
            else
                Mobile = UserId;
            DbParam[] param = new DbParam[]
            {
                new DbParam(Mobile, typeof(System.String), "_MobileNo"),
                new DbParam(Email, typeof(System.String), "_EmailId")
            };

            DataSet ds = db.GetDataset("sp_GetUserAssosiatedDetail", param);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["ConnectionString"] != DBNull.Value)
                    ConnectionString = dr["ConnectionString"].ToString();
                if (dr["UserFullName"] != DBNull.Value)
                    UserFullName = dr["UserFullName"].ToString();
                else
                    UserFullName = "NA";
                if (dr["SchoolName"] != DBNull.Value)
                    SchoolName = dr["SchoolName"].ToString();
                else
                    SchoolName = "NA";
                Token = beanContext.AddNewSession(UserId + UserFullName + SchoolName, null, null, null);
            }
            return Token;
        }
    }
}

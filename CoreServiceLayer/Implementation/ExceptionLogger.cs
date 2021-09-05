using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using ServiceLayer.Interface;
using System;

namespace CoreServiceLayer.Implementation
{
    public class ExceptionLogger : CurrentUserObject, IExceptionLogger<ExceptionLogger>
    {
        private readonly IDb db;

        public ExceptionLogger(IDb db, CurrentSession currentSession)
        {
            this.userDetail = currentSession.CurrentUserDetail;
            this.db = db;
        }

        public Boolean LogException(string Token, Boolean IsException, string ExceptionMessage, Boolean IsDestroyed)
        {
            Boolean Flag = false;
            DateTime? DestroyedDateTime = null;
            if (IsDestroyed)
                DestroyedDateTime = DateTime.Now;
            if (this.userDetail != null)
            {
                DbParam[] SessionParam = new DbParam[]
                {
                    new DbParam(Token, typeof(System.String), "_Token"),
                    new DbParam(DateTime.Now, typeof(System.DateTime), "_CreatedOn"),
                    new DbParam(DateTime.Now, typeof(System.DateTime), "_LastUpdatedOn"),
                    new DbParam(0, typeof(System.Double), "_Duration"),
                    new DbParam(DestroyedDateTime, typeof(System.DateTime), "_DestroyedOn"),
                    new DbParam(IsException, typeof(System.Boolean), "_IsException"),
                    new DbParam(ExceptionMessage, typeof(System.String), "_ExceptionMessage"),
                    new DbParam("", typeof(System.String), "_UserName"),
                    new DbParam(this.userDetail.Mobile, typeof(System.String), "_UserMobileNo")
                };
                var Result = db.ExecuteNonQuery("sp_TrackSessionObject_InsUpd", SessionParam, true);
                if (!string.IsNullOrEmpty(Result) && (Convert.ToInt32(Result) == 100 || Convert.ToInt32(Result) == 101))
                {
                    Flag = true;
                }
            }
            return Flag;
        }
    }
}

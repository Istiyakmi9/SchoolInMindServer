using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ORMModels;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Collections.Generic;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class ExamDataService : CurrentUserObject, IExamDataService<ExamDataService>
    {
        private readonly IDb db;
        private readonly IValidateModalService<ValidateModalService> validateModalService;

        public ExamDataService(ValidateModalService validateModalService, IDb db, CurrentSession currentSession)
        {
            this.db = db;
            this.validateModalService = validateModalService;
            userDetail = currentSession.CurrentUserDetail;
        }

        public DataSet ExamDetailService(string Class, string ExamDescriptionUid)
        {
            ResultSet = null;
            if (!string.IsNullOrEmpty(Class) && !string.IsNullOrEmpty(ExamDescriptionUid))
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(Class, typeof(System.String), "_Class"),
                    new DbParam(userDetail.TenentId, typeof(System.String), "_TanentId"),
                    new DbParam(ExamDescriptionUid, typeof(System.String), "_ExamDescriptionUid")
                };
                ResultSet = db.GetDataset("sp_ExamDetails_Sel", param);
            }

            return ResultSet;
        }

        public string GetExamDetails(int examId, int year, string tenentId)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(tenentId, typeof(System.String), "_tenentId"),
                new DbParam(examId, typeof(System.String), "_examId"),
                new DbParam(year, typeof(System.Int32), "_year")
            };

            DataSet ds = db.GetDataset("sp_ExamDetails_ByExamId", param);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ResultSet = JsonConvert.SerializeObject(ds);
            }

            return ResultSet;
        }

        public string ExamDetailUpdateService(List<Examdetails> examdetails)
        {
            string Result = "";
            if (examdetails != null && examdetails.Count > 0)
            {
                foreach (var Item in examdetails)
                {
                    Item.AdminUid = this.userDetail.UserId;
                    Item.TanentUid = this.userDetail.TenentId;
                }

                DataSet ds = this.beanContext.ConvertToDataSet<Examdetails>(examdetails);
                Result = db.InsertUpdateBatchRecord("sp_ExamDetailsUpdate_Upd", ds.Tables[0]);
            }
            return Result;
        }
    }
}

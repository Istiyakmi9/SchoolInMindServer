using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.ORMModels;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Collections.Generic;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class ExamResultService : CurrentUserObject, IExamResultService<ExamResultService>
    {
        private readonly IDb db;

        public ExamResultService(UserDetail userDetail, IDb db)
        {
            this.db = db;
            this.userDetail = userDetail;
        }

        public string GetResultById(int ExamId, string StudentId)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId"),
                new DbParam(ExamId, typeof(System.Int32), "_ExamId"),
                new DbParam(StudentId, typeof(System.String), "_StudentId")
            };

            DataSet ds = db.GetDataset("sp_GetExamResult_ById", param);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ResultSet = JsonConvert.SerializeObject(ds);
            }

            return ResultSet;
        }

        public string GetResultByRegistrationNo(ExamResultPostData objExamResultPostData)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(objExamResultPostData.RegistrationNo, typeof(System.String), "_registrationNo"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentUid"),
                new DbParam(objExamResultPostData.AcedimicYearFrom, typeof(System.Int32), "_academicYear"),
                new DbParam(objExamResultPostData.ExamId, typeof(System.Int32), "_examId"),
            };

            string ProcessingStatue = string.Empty;
            DataSet ds = db.GetDataset("sp_AcademicExamResult_GetByRegNo", param, true, ref ProcessingStatue);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ResultSet = JsonConvert.SerializeObject(ds);
            }

            return ResultSet;
        }

        public string AddEditResultService(ExamResult ObjExamResult)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(ObjExamResult.ExamResultId, typeof(System.String), "_ExamResultId"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid"),
                new DbParam(ObjExamResult.ExamDescriptionId, typeof(System.String), "_ExamDescriptionId"),
                new DbParam(ObjExamResult.StudentUid, typeof(System.String), "_StudentUid"),
                new DbParam(ObjExamResult.SubjectUid, typeof(System.String), "_SubjectUid"),
                new DbParam(ObjExamResult.Marks, typeof(System.Int64), "_Marks"),
                new DbParam(ObjExamResult.Grade, typeof(System.Int32), "_Grade"),
                new DbParam(userDetail.AccedemicStartYear, typeof(System.Int32), "_AcedemicYearFrom")
            };

            ResultSet = db.ExecuteNonQuery("sp_Examresult_InsUpd", param, true);
            return ResultSet;
        }


        public string ViewResultDataService(string SearchStr, string ShortBy, int PageIndex, int PageSize)
        {
            string ResultSet = null;
            if (string.IsNullOrEmpty(SearchStr))
                SearchStr = "1=1";
            if (string.IsNullOrEmpty(ShortBy))
                ShortBy = "1=1";
            if (PageIndex <= 0)
                PageIndex = 1;
            if (PageSize <= 0)
                PageSize = 10;

            DbParam[] param = new DbParam[]
            {
                new DbParam(SearchStr, typeof(System.String), "_SearchStr"),
                new DbParam(ShortBy, typeof(System.String), "_ShortBy"),
                new DbParam(PageIndex, typeof(System.Int32), "_PageIndex"),
                new DbParam(PageSize, typeof(System.Int32), "_PageSize"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid")
            };

            string ProcessingStatus = string.Empty;
            DataSet ds = db.GetDataset("sp_ExamResult_SelFilter", param, true, ref ProcessingStatus);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ResultSet = JsonConvert.SerializeObject(ds);
            }

            return ResultSet;
        }

        public string ShowExamResult(string StudentId, string TenentId)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId"),
                new DbParam(StudentId, typeof(System.String), "_StudentId")
            };

            IList<ShowResultData> objShowResultData = new List<ShowResultData>();
            objShowResultData.Add(new ShowResultData()
            {
                AcademicYearFrom = 2017,
                AcademicYearTo = 2018,
                ExamId = 300,
                ExamName = "Final Exam",
                ExamStartDate = "02/03/2018",
                ResultPercentage = 75
            });

            objShowResultData.Add(new ShowResultData()
            {
                AcademicYearFrom = 2017,
                AcademicYearTo = 2018,
                ExamId = 301,
                ExamName = "Third term unit test",
                ExamStartDate = "02/01/2018",
                ResultPercentage = 82
            });

            objShowResultData.Add(new ShowResultData()
            {
                AcademicYearFrom = 2017,
                AcademicYearTo = 2018,
                ExamId = 200,
                ExamName = "Half-yearly exam",
                ExamStartDate = "02/05/2018",
                ResultPercentage = 86
            });

            ResultSet = JsonConvert.SerializeObject(objShowResultData);
            return ResultSet;
        }
    }
}

using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class ReportService : CurrentUserObject, IReportService<ReportService>
    {
        private readonly IDb db;
        private readonly ICommonService<CommonService> commonService;

        public ReportService(CommonService commonService, IDb db, CurrentSession currentSession)
        {
            this.db = db;
            this.commonService = commonService;
            userDetail = currentSession.CurrentUserDetail;
        }

        public string StudentReportService(SearchModal searchModal)
        {
            this.commonService.ValidateFilterModal(searchModal, "studentUid");
            return ExecuteReportGenericService<Student>(searchModal, "sp_StudentDetail_SelByFilter");
        }

        public string GuardianReportService(SearchModal searchModal)
        {
            this.commonService.ValidateFilterModal(searchModal, "parentdetailid");
            return ExecuteReportGenericService<Faculty>(searchModal, "sp_parentdetail_SelByFilter");
        }

        public string StaffReportService(SearchModal searchModal)
        {
            this.commonService.ValidateFilterModal(searchModal, "StaffMemberUid");
            return ExecuteReportGenericService<Faculty>(searchModal, "sp_OtherStaffMembers_SelByFilter");
        }

        public string FacultyReportService(SearchModal searchModal)
        {
            this.commonService.ValidateFilterModal(searchModal, "StaffMemberUid");
            return ExecuteReportGenericService<Faculty>(searchModal, "sp_Faculty_SelByFilter");
        }

        public string ExecuteReportGenericService<T>(SearchModal searchModal, string ProcedureName)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.Int32), "_pageSize")
            };

            DataSet ds = db.GetDataset(ProcedureName, param);
            ds = UiMappedColumn.BuildColumn<T>(ds);
            return JsonConvert.SerializeObject(ds);
        }

        public DataSet ParentDetailByMobileService(string StudentUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(StudentUid, typeof(System.String), "_StudentUid"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_TenentId"),
            };

            DataSet ds = db.GetDataset("sp_ParentDetail_ByStudentUid", param);
            return ds;
        }
    }
}

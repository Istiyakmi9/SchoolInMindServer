using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class MarkService : CurrentUserObject, IMarkService
    {
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;

        public MarkService(IDb db, ValidateModalService validateModalService, UserDetail userDetail, CurrentSession currentSession)
        {
            this.db = db;
            this.userDetail = currentSession.CurrentUserDetail;
            this.validateModalService = validateModalService;
        }

        public string FetchMarksService(SearchModal searchModal)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.Int32), "_pageSize")
            };

            DataSet ds = db.GetDataset("sp_Marks_SelFilter", param);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }

        public string ManageMarksService(Holidays holidays)
        {

            return null;
        }

        public string DeleteMarksService(Holidays holidays)
        {

            return null;
        }
    }
}

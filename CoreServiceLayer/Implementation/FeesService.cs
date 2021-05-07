using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class FeesService : CurrentUserObject, IFeesService
    {
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;

        public FeesService(IDb db, ValidateModalService validateModalService, UserDetail userDetail, CurrentSession currentSession)
        {
            this.db = db;
            this.userDetail = currentSession.CurrentUserDetail;
            this.validateModalService = validateModalService;
        }

        public string FetchFeesService(SearchModal searchModal)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.Int32), "_pageSize")
            };

            DataSet ds = db.GetDataset("sp_PaymentDetail_SelFilter", param);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }

        public string ManageFeesService(Holidays holidays)
        {

            return null;
        }

        public string DeleteFeesService(Holidays holidays)
        {

            return null;
        }
    }
}

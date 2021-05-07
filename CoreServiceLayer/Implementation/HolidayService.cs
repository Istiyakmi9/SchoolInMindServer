using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class HolidayService : CurrentUserObject, IHolidayService
    {
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;

        public HolidayService(IDb db, ValidateModalService validateModalService, UserDetail userDetail, CurrentSession currentSession)
        {
            this.db = db;
            this.userDetail = currentSession.CurrentUserDetail;
            this.validateModalService = validateModalService;
        }

        public string GetHolidayListService(SearchModal searchModal)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.Int32), "_pageSize")
            };

            DataSet ds = db.GetDataset("sp_holidays_SelFilter", param);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }

        public string ManageHolidayService(Holidays holidays)
        {

            return null;
        }

        public string DeleteHolidayService(Holidays holidays)
        {

            return null;
        }
    }
}

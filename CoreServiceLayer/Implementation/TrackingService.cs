using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.FactoryContext;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class TrackingService : CurrentUserObject, ITrackingService<TrackingService>
    {
        private readonly IDb db;

        public TrackingService(IDb db, UserDetail userDetail)
        {
            this.db = db;
        }

        public string GetMapDetailService(string DeviceId)
        {
            return null;
        }
        public string GetVehicleService(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(SearchStr, typeof(System.String), "_searchString"),
                new DbParam(SortBy, typeof(System.String), "_sortBy"),
                new DbParam(PageIndex, typeof(System.String), "_pageIndex"),
                new DbParam(PageSize, typeof(System.String), "_pageSize")
            };
            DataSet ds = db.GetDataset("sp_VehicleDetail_Sel", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ResultSet = JsonConvert.SerializeObject(ds);
            }
            return ResultSet;
        }
    }
}

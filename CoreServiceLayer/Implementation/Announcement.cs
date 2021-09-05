using BottomhalfCore.Annotations;
using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class AnnouncementService : CurrentUserObject, IAnnouncementService
    {
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;

        public AnnouncementService(IDb db, UserDetail userDetail, ValidateModalService validateModalService, CurrentSession currentSession)
        {
            this.db = db;
            this.userDetail = currentSession.CurrentUserDetail;
            this.validateModalService = validateModalService;
        }

        public string GetAllAnnouncementService(FetchAnnouncement fetchAnnouncement)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(fetchAnnouncement.searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(fetchAnnouncement.searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(fetchAnnouncement.searchModal.PageIndex, typeof(System.Int32), "_pageIndex"),
                new DbParam(fetchAnnouncement.searchModal.PageSize, typeof(System.Int32), "_pageSize"),
                new DbParam(fetchAnnouncement.StudentUid, typeof(System.String), "_studentUid"),
                new DbParam(fetchAnnouncement.ClassDetailUid, typeof(System.String), "_classDetailUid"),
                new DbParam(this.userDetail.TenentId, typeof(System.String), "_tenentId")
            };

            DataSet ds = db.GetDataset("sp_Announcement_SelFilter", param);
            if (ds != null && ds.Tables.Count > 0)
            {
                ds = UiMappedColumn.BuildColumn<Announcement>(ds);
                ResultSet = JsonConvert.SerializeObject(ds);
            }
            return ResultSet;
        }
    }
}

using BottomhalfCore.Annotations;
using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using MultiTypeDocumentConverter.Service;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreServiceLayer.Implementation
{
    public class ApplicationSettingService : CurrentUserObject, IApplicationSettingService<ApplicationSettingService>
    {
        private string Result = string.Empty;
        private readonly IDb db;
        private readonly IDocumentConverter _documentConverter;

        public ApplicationSettingService(IDb db, CurrentSession currentSession, IDocumentConverter documentConverter)
        {
            this.db = db;
            userDetail = currentSession.CurrentUserDetail;
            _documentConverter = documentConverter;
        }

        public DataSet DeleteService(int storeIds)
        {
            DataSet ds = null;
            if (storeIds > 0)
            {
                DbParam[] dbParam = new DbParam[]
                {
                    new DbParam(storeIds, typeof(string), "_storeId")
                };

                Result = this.db.ExecuteNonQuery("sp_StoreZone_Del", dbParam, false);
                if (Result != "Failed" || Result != null)
                {
                    ds = GetZone();
                }
            }
            return ds;
        }

        public string CreateOrUpdateServices(List<StoreZone> storeZone)
        {
            if (storeZone.Where(x => x.ZoneName == null || x.ZoneName == "").FirstOrDefault() == null)
            {
                if (storeZone.Count() > 0)
                {
                    Parallel.ForEach(storeZone, Item =>
                    {
                        Item.AdminUid = this.userDetail.UserId;
                        Item.TanentUid = this.userDetail.TenentId;
                    });

                    ResultSet = this.beanContext.ConvertToDataSet<StoreZone>(storeZone);
                    if (ResultSet.Tables[0].Rows.Count > 0)
                    {
                        Result = this.db.InsertUpdateBatchRecord("sp_StoreZone_InsUpd", ResultSet.Tables[0]);
                    }
                }
            }

            return Result;
        }

        public DataSet GetZone()
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(this.userDetail.TenentId, typeof(System.String), "_TanentUid")
            };

            ResultSet = this.db.GetDataset("sp_StoreZone_Sel", param);
            return ResultSet;
        }

        public string CreateRoomService(int RoomsCount)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(RoomsCount, typeof(System.Int32), "_RoomsCount"),
                new DbParam(this.userDetail.TenentId, typeof(System.String), "_TanentUid"),
                new DbParam(this.userDetail.UserId, typeof(System.String), "_AdminUid")
            };

            ResultSet = this.db.GetDataset("sp_Rooms_Create", param);
            return JsonConvert.SerializeObject(ResultSet);
        }

        public string GetRoomService(SearchModal searchModal)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.String), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.String), "_pageSize")
            };

            ResultSet = this.db.GetDataset("sp_Rooms_SelByFilter", param);
            if (this.beanContext.IsValidDataSet(ResultSet))
            {
                Result = JsonConvert.SerializeObject(ResultSet);
            }
            return Result;
        }

        public string UpdateRoomDetailService(RoomDetail roomDetail)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(roomDetail.RoomUid, typeof(System.String), "_RoomUid"),
                new DbParam(roomDetail.RoomNo, typeof(System.Int64), "_RoomNo"),
                new DbParam(roomDetail.ClassDetailUid, typeof(System.String), "_ClassDetailUid"),
                new DbParam(roomDetail.RoomType, typeof(System.String), "_RoomType"),
                new DbParam(this.userDetail.UserId, typeof(System.String), "_AdminUid"),
                new DbParam(this.userDetail.TenentId, typeof(System.String), "_TanentUid")
            };

            Result = this.db.ExecuteNonQuery("sp_Rooms_InsUpd", param, true);
            if (Result.IndexOf("successfully") != -1)
            {
                SearchModal searchModal = new SearchModal
                {
                    PageIndex = 1,
                    PageSize = 20,
                    SearchString = "1=1",
                    SortBy = ""
                };
                Result = GetRoomService(searchModal);
            }
            return Result;
        }

        public string GetHtml(string FileRelativePath)
        { 
            string ActualFolderPath = Path.Combine(this.beanContext.GetContentRootPath(), FileRelativePath);
            return _documentConverter.DocToHtml(ActualFolderPath);
        }
    }
}

using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Data;
using System.Linq;

namespace CoreServiceLayer.Implementation
{
    public class GoodsItemService : CurrentUserObject, IGoodsItemService<GoodsItemService>
    {
        public string ConnectionString = null;
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;

        public GoodsItemService(UserDetail userDetail, ValidateModalService validateModalService)
        {
            this.validateModalService = validateModalService;
            userDetail = userDetail;
        }

        public string ItemList(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            if (SortBy == null || SortBy == "")
                SortBy = " StaffMemberUid";
            return ExecuteItemReportService(SearchString, SortBy, PageIndex, PageSize, "sp_vendor_SelByFilter");
        }

        public string GoodsItemFilterService(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            if (SortBy == null || SortBy == "")
                SortBy = " GoodsItemUid";
            return ExecuteItemReportService(SearchString, SortBy, PageIndex, PageSize, "sp_GoodsItem_SelByFilter");
        }

        public string ClientFilterService(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            if (SortBy == null || SortBy == "")
                SortBy = " ClientUid";
            return ExecuteItemReportService(SearchString, SortBy, PageIndex, PageSize, "sp_Clients_SelByFilter");
        }

        public string ExecuteItemReportService(string SearchString, string SortBy, string PageIndex, string PageSize, string ProcedureName)
        {
            string ResultSet = null;
            if (SearchString == null || SearchString == "")
                SearchString = " 1=1";
            if (PageIndex == null || PageIndex == "")
                PageIndex = "1";
            if (PageSize == null || PageSize == "")
                PageSize = "10";
            DbParam[] param = new DbParam[]
            {
                new DbParam(SearchString, typeof(System.String), "_searchString"),
                new DbParam(SortBy, typeof(System.String), "_sortBy"),
                new DbParam(PageIndex, typeof(System.String), "_pageIndex"),
                new DbParam(PageSize, typeof(System.String), "_pageSize")
            };

            DataSet ds = db.GetDataset(ProcedureName, param);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }

        public string GetVendorDetail()
        {
            string ResultSet = null;
            DataSet ds = db.GetDataset("sp_vendor_SelAll", null);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }

        public string GoodsItemByUidService(string GoodsItemUid)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(GoodsItemUid, typeof(System.String), "_goodsItemUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentUid"),
            };
            DataSet ds = db.GetDataset("sp_GoodsItem_ByUid", param);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }

        public string AddNewGoodsItem(Vendor ObjGoods)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(ObjGoods.ExistingVendorUid, typeof(System.String), "_ExistingVendorUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId"),
                new DbParam(ObjGoods.SellerFirstName, typeof(System.String), "_SellerFirstName"),
                new DbParam(ObjGoods.SellerLastName, typeof(System.String), "_SellerLastName"),
                new DbParam(ObjGoods.ShopName, typeof(System.String), "_ShopName"),
                new DbParam(ObjGoods.FullAddress, typeof(System.String), "_FullAddress"),
                new DbParam(ObjGoods.Mobile, typeof(System.String), "_Mobile"),
                new DbParam(ObjGoods.Email, typeof(System.String), "_Email"),
                new DbParam(ObjGoods.GSTIN, typeof(System.String), "_GSTIN"),
                new DbParam(ObjGoods.BankName, typeof(System.String), "_BankName"),
                new DbParam(ObjGoods.AccountNo, typeof(System.String), "_AccountNo"),
                new DbParam(ObjGoods.PurchasedAmount, typeof(System.String), "_PurchasedAmount"),
                new DbParam(ObjGoods.IFSCCode, typeof(System.String), "_IFSCCode"),
                new DbParam(ObjGoods.GoodsAsXml, typeof(System.String), "_GoodsAsXml"),
                new DbParam(ObjGoods.InvoiceNo, typeof(System.String), "_InvoiceNo"),
                new DbParam(userDetail.UserId, typeof(System.String), "_AdminUid")
            };

            ResultSet = db.ExecuteNonQuery("sp_VendorGoods_AddItem", param, true);
            return ResultSet;
        }

        public string GetItemPreFetchDetailService()
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentUid")
            };

            string ProcessingStatus = string.Empty;
            DataSet ds = db.GetDataset("sp_GoodsItemAndTypes", param, true, ref ProcessingStatus);
            if (ds != null && ds.Tables.Count > 0)
            {
                var Result = (from n in ds.Tables[0].AsEnumerable()
                              group n by new { Item = n.Field<string>("Item") } into g
                              select new
                              {
                                  Name = g.Key.Item,
                                  Data = g
                              }).ToList();

                ResultSet = JsonConvert.SerializeObject(Result);
            }
            return ResultSet;
        }

        public ServiceResult AddClientService(Clients ObjClient)
        {
            string ReturnedMessage = null;
            ServiceResult ObjServiceResult = this.validateModalService.ValidateModalFieldsService(typeof(Clients), ObjClient);
            if (ObjServiceResult.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(ObjClient.ClientUid , typeof(System.String), "_ClientUid"),
                    new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId"),
                    new DbParam(ObjClient.GSTIN, typeof(System.String), "_GSTINDetail"),
                    new DbParam(ObjClient.AdharNumber, typeof(System.String), "_AdharNumber"),
                    new DbParam(ObjClient.PersonName, typeof(System.String), "_PersonName"),
                    new DbParam(ObjClient.ShopName, typeof(System.String), "_ShopName"),
                    new DbParam(ObjClient.FullAddress, typeof(System.String), "_FullAddress"),
                    new DbParam(ObjClient.Mobile, typeof(System.String), "_Mobile"),
                    new DbParam(ObjClient.Email, typeof(System.String), "_Email"),
                };

                ReturnedMessage = db.ExecuteNonQuery("sp_Clients_InsUpt", param, true);
                if (ReturnedMessage == "100" || ReturnedMessage == "101")
                {
                    ObjServiceResult.StatusCode = Convert.ToInt32(ReturnedMessage);
                }
                else
                {
                    ObjServiceResult.StatusCode = -1;
                    ObjServiceResult.ErrorMessage = ReturnedMessage;
                }
            }
            return ObjServiceResult;
        }

        public string GetClientByUidService(string ClientUid, string TenentId)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(ClientUid, typeof(System.String), "_clientUid"),
                new DbParam(TenentId, typeof(System.String), "_tenentUid"),
            };

            string ProcessingStatus = string.Empty;
            var ds = db.GetDataset("sp_Client_ByUid", param, true, ref ProcessingStatus);
            ResultSet = JsonConvert.SerializeObject(ds);
            return ResultSet;
        }

        public ServiceResult UpdateSingleGoodService(GoodsItem ObjGoodsItem)
        {
            string ReturnedMessage = null;
            ServiceResult ObjServiceResult = validateModalService.ValidateModalFieldsService(typeof(GoodsItem), ObjGoodsItem);
            if (ObjServiceResult.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(ObjGoodsItem.GoodsItemUid, typeof(System.String), "_GoodsItemUid"),
                    new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId"),
                    new DbParam(ObjGoodsItem.GoodsUid, typeof(System.String), "_GoodsUid"),
                    new DbParam(ObjGoodsItem.Item, typeof(System.String), "_Item"),
                    new DbParam(ObjGoodsItem.ItemName, typeof(System.String), "_ItemName"),
                    new DbParam(ObjGoodsItem.ItemCompanyName, typeof(System.String), "_ItemCompanyName"),
                    new DbParam(ObjGoodsItem.HSNCode, typeof(System.String), "_HSNCode"),
                    new DbParam(ObjGoodsItem.salePrice, typeof(System.String), "_salePrice"),
                    new DbParam(ObjGoodsItem.PurchasedPrice, typeof(System.String), "_PurchasedPrice"),
                    new DbParam(ObjGoodsItem.MRP, typeof(System.String), "_MRP"),
                    new DbParam(ObjGoodsItem.Unit, typeof(System.String), "_Unit"),
                    new DbParam(ObjGoodsItem.M_Qty, typeof(System.String), "_M_Qty"),
                    new DbParam(ObjGoodsItem.Discount, typeof(System.String), "_Discount"),
                    new DbParam(ObjGoodsItem.GST, typeof(System.String), "_GST"),
                    new DbParam(ObjGoodsItem.BatchNo, typeof(System.String), "_BatchNo")
                };

                ReturnedMessage = db.ExecuteNonQuery("sp_GoodsItem_InsUpt", param, true);
                if (ReturnedMessage == "100" || ReturnedMessage == "101")
                {
                    ObjServiceResult.StatusCode = Convert.ToInt32(ReturnedMessage);
                    ObjServiceResult.SuccessMessage = "Inserted/Updated successfully";
                }
                else
                {
                    ObjServiceResult.StatusCode = -1;
                    ObjServiceResult.ErrorMessage = ReturnedMessage;
                }
            }
            return ObjServiceResult;
        }
    }
}
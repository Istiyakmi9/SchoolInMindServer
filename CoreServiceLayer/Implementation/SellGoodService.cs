using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class SellGoodService : CurrentUserObject, ISellGoodService<SellGoodService>
    {
        private readonly UserDefineMapping userDefineMapping;
        private readonly IDb db;

        public SellGoodService(UserDefineMapping userDefineMapping)
        {
            this.userDefineMapping = userDefineMapping;
        }

        public string InsertClientDetail(SoldItems ObjSoldItems)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(ObjSoldItems.AdharNumber, typeof(System.String), "_AdharNumber"),
                new DbParam(ObjSoldItems.PersonName, typeof(System.String), "_PersonName"),
                new DbParam(ObjSoldItems.ShopName, typeof(System.String), "_ShopName"),
                new DbParam(ObjSoldItems.BankName, typeof(System.String), "_BankName"),
                new DbParam(ObjSoldItems.AccountNo, typeof(System.String), "_AccountNo"),
                new DbParam(ObjSoldItems.IFSCCode, typeof(System.String), "_IFSCCode"),
                new DbParam(ObjSoldItems.GSTIN, typeof(System.String), "_GSTIN"),
                new DbParam(ObjSoldItems.FullAddress, typeof(System.String), "_FullAddress"),
                new DbParam(ObjSoldItems.Mobile, typeof(System.String), "_Mobile"),
                new DbParam(ObjSoldItems.Email, typeof(System.String), "_Email"),
                new DbParam(ObjSoldItems.ExistingClientUid, typeof(System.String), "_ExistingClientUid"),
                new DbParam(ObjSoldItems.FileName, typeof(System.String), "_FileName"),
                new DbParam(DateTime.Now, typeof(System.DateTime), "_OrderedDate"),
                new DbParam(ObjSoldItems.NewShippingAddress, typeof(System.String), "_NewShippingAddress"),
                new DbParam(ObjSoldItems.AddressUniqueCode, typeof(System.String), "_AddressUniqueCode"),
                new DbParam(ObjSoldItems.PaymentMode, typeof(System.String), "_PaymentMode"),
                new DbParam(ObjSoldItems.GuestUserName, typeof(System.String), "_GuestUserName"),
                new DbParam(ObjSoldItems.PaymentType, typeof(System.String), "_PaymentType"),
                new DbParam(ObjSoldItems.Latitude, typeof(System.String), "_Latitude"),
                new DbParam(ObjSoldItems.Longitude, typeof(System.String), "_Longitude"),
                new DbParam(ObjSoldItems.Mihpayid, typeof(System.String), "_Mihpayid"),
                new DbParam(ObjSoldItems.BankStatus, typeof(System.String), "_BankStatus"),
                new DbParam(ObjSoldItems.UnmappedStatus, typeof(System.String), "_UnmappedStatus"),
                new DbParam(ObjSoldItems.OrderStatus, typeof(System.String), "_OrderStatus"),
                new DbParam(ObjSoldItems.GoodsAsXml, typeof(System.String), "_GoodsAsXml")
            };
            db.ExecuteNonQuery("", param, false);
            return ResultSet;
        }

        public SoldItems SoldGoodsService(SoldItems ObjSoldItems)
        {
            if (!string.IsNullOrEmpty(ObjSoldItems.ExistingClientUid))
            {
                Clients ObjClients = GetClientDetailByUid(ObjSoldItems.ExistingClientUid);
                ObjSoldItems.Mobile = ObjClients.Mobile;
                ObjSoldItems.Email = ObjClients.Email;
                ObjSoldItems.PersonName = ObjClients.PersonName;
            }

            return ObjSoldItems;
        }

        public Clients GetClientDetailByUid(string ExistingClientUid)
        {
            Clients ClientResult = null;
            String ResultSet = null;
            ClientResult = new Clients();
            DbParam[] param = new DbParam[]
            {
                new DbParam(ExistingClientUid, typeof(System.String), "_clientUid"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_tenentUid")
            };

            DataSet ds = db.GetDataset("sp_Client_ByUid", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ClientResult = userDefineMapping.ConvertToObject(ds.Tables[0], ClientResult.GetType(), out ResultSet) as Clients;
                if (ResultSet != "100")
                    return null;
            }
            return ClientResult;
        }

        public string GetClassesService(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            string ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(SearchStr, typeof(System.String), "_searchString"),
                new DbParam(SortBy, typeof(System.String), "_sortBy"),
                new DbParam(PageIndex, typeof(System.String), "_pageIndex"),
                new DbParam(PageSize, typeof(System.String), "_pageSize")
            };
            DataSet ds = db.GetDataset("sp_ListClassDetail_BySectionGroup", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ResultSet = JsonConvert.SerializeObject(ds);
            }
            return ResultSet;
        }

        public string GetClientInfoService()
        {
            string ResultSet = null;

            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.TenentId, typeof(System.String), "_tenentId")
            };
            DataSet ds = db.GetDataset("sp_Clients_GetAll", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ResultSet = JsonConvert.SerializeObject(ds);
            }
            return ResultSet;
        }
    }
}

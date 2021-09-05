using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.Services.Code;
using BottomhalfCore.Services.Interface;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using ServiceLayer.Interface;
using System;
using System.Data;

namespace CoreServiceLayer.Implementation
{
    public class VendorService : CurrentUserObject, IVendorService<VendorService>
    {
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly IDb db;
        private readonly IAutoMapper<TableAutoMapper> mapper;
        public VendorService(ValidateModalService validateModalService, CurrentSession currentSession)
        {
            this.validateModalService = validateModalService;
            this.mapper = new TableAutoMapper();
            this.userDetail = currentSession.CurrentUserDetail;
        }

        public ServiceResult RegisterVendorService(Vendor objVendor)
        {
            string ReturnedMessage = null;
            ServiceResult ObjServiceResult = this.validateModalService.ValidateModalFieldsService(typeof(Vendor), objVendor);
            if (ObjServiceResult.IsValidModal)
            {
                DbParam[] param = new DbParam[]
                {
                    new DbParam(objVendor.VendorUId, typeof(System.String), "_vendoruid"),
                    new DbParam(userDetail.TenentId, typeof(System.String), "_tenentid"),
                    new DbParam(objVendor.SellerFirstName, typeof(System.String), "_sellerfirstname"),
                    new DbParam(objVendor.SellerLastName, typeof(System.String), "_sellerlastname"),
                    new DbParam(objVendor.ShopName, typeof(System.String), "_shopname"),
                    new DbParam(objVendor.FullAddress, typeof(System.String), "_fulladdress"),
                    new DbParam(objVendor.Mobile, typeof(System.String), "_mobile"),
                    new DbParam(objVendor.Email, typeof(System.String), "_email"),
                    new DbParam(objVendor.GSTIN, typeof(System.String), "_gstin"),
                    new DbParam(objVendor.BankName, typeof(System.String), "_bankname"),
                    new DbParam(objVendor.AccountNo, typeof(System.String), "_accountno"),
                    new DbParam(objVendor.PurchasedAmount, typeof(System.Double), "_purchasedamount"),
                    new DbParam(objVendor.IFSCCode, typeof(System.String), "_ifsccode"),
                    new DbParam(userDetail.UserId, typeof(System.String), "_adminid"),
                };

                ReturnedMessage = db.ExecuteNonQuery("sp_vendor_InsUpd", param, true);
                if (ReturnedMessage == "100" || ReturnedMessage == "101")
                    ObjServiceResult.StatusCode = Convert.ToInt32(ReturnedMessage);
                else
                {
                    ObjServiceResult.StatusCode = -1;
                    ObjServiceResult.ErrorMessage = ReturnedMessage;
                }
            }
            return ObjServiceResult;
        }

        public Vendor GetVendorByUid(string vendorUId)
        {
            Vendor objVendor = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(vendorUId, typeof(System.String), "_vendorUid"),
                new DbParam(userDetail.TenentId, typeof(System.String), "_tenentUid")
            };

            string ProcessingStatus = string.Empty;
            DataSet ds = db.GetDataset("sp_vendor_SelByUid", param, true, ref ProcessingStatus);
            if (ds != null && ds.Tables.Count > 0)
                objVendor = null;// mapper.AutoMapToObject(ds.Tables[0], "Vendor", out Message) as Vendor;
            return objVendor;
        }
    }
}
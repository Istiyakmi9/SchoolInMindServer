using BottomhalfCore.FactoryContext;
using BottomhalfCore.Annotations;
using CommonModal.Models;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;
using System;
using System.Threading.Tasks;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class SellGoodsController : ControllerBase
    {
        // GET: SellGoods
        private BeanContext context;
        private readonly ISellGoodService<SellGoodService> sellGoodService;
        private readonly IAccountService<AccountService> accountService;
        public SellGoodsController(SellGoodService sellGoodService, AccountService accountService)
        {
            this.sellGoodService = sellGoodService;
            this.accountService = accountService;
            this.context = BeanContext.GetInstance();
        }

        public IResponse<ApiResponse> SellNow()
        {
            //ViewBag.ClientRecord = sellGoodService.GetClientInfoService();
            return null;
        }

        public async Task<IResponse<ApiResponse>> PayUReturnAction(FormCollection form)
        {
            try
            {
                PayUResponse ObjPayUResponse = new PayUResponse();
                if (form.Keys.Count > 0)
                {
                    ObjPayUResponse.mihpayid = form["mihpayid"].ToString();
                    ObjPayUResponse.mode = form["mode"].ToString();
                    ObjPayUResponse.status = form["status"].ToString();
                    ObjPayUResponse.unmappedstatus = form["unmappedstatus"].ToString();
                    ObjPayUResponse.key = form["key"].ToString();
                    ObjPayUResponse.txnid = form["txnid"].ToString();
                    ObjPayUResponse.amount = form["amount"].ToString();
                    ObjPayUResponse.addedon = form["addedon"].ToString();
                    ObjPayUResponse.productinfo = form["productinfo"].ToString();
                    ObjPayUResponse.firstname = form["firstname"].ToString();
                    ObjPayUResponse.lastname = form["lastname"].ToString();
                    ObjPayUResponse.address1 = form["address1"].ToString();
                    ObjPayUResponse.address2 = form["address2"].ToString();
                    ObjPayUResponse.city = form["city"].ToString();
                    ObjPayUResponse.state = form["state"].ToString();
                    ObjPayUResponse.country = form["country"].ToString();
                    ObjPayUResponse.zipcode = form["zipcode"].ToString();
                    ObjPayUResponse.email = form["email"].ToString();
                    ObjPayUResponse.phone = form["phone"].ToString();
                    ObjPayUResponse.udf1 = form["udf1"].ToString();
                    ObjPayUResponse.udf2 = form["udf2"].ToString();
                    ObjPayUResponse.udf3 = form["udf3"].ToString();
                    ObjPayUResponse.udf4 = form["udf4"].ToString();
                    ObjPayUResponse.udf5 = form["udf5"].ToString();
                    ObjPayUResponse.udf6 = form["udf6"].ToString();
                    ObjPayUResponse.udf7 = form["udf7"].ToString();
                    ObjPayUResponse.udf8 = form["udf8"].ToString();
                    ObjPayUResponse.udf9 = form["udf9"].ToString();
                    ObjPayUResponse.udf10 = form["udf10"].ToString();
                    ObjPayUResponse.hash = form["hash"].ToString();
                    ObjPayUResponse.field1 = form["field1"].ToString();
                    ObjPayUResponse.field2 = form["field2"].ToString();
                    ObjPayUResponse.field3 = form["field3"].ToString();
                    ObjPayUResponse.field4 = form["field4"].ToString();
                    ObjPayUResponse.field5 = form["field5"].ToString();
                    ObjPayUResponse.field6 = form["field6"].ToString();
                    ObjPayUResponse.field7 = form["field7"].ToString();
                    ObjPayUResponse.field8 = form["field8"].ToString();
                    ObjPayUResponse.field9 = form["field9"].ToString();
                    ObjPayUResponse.PG_TYPE = form["PG_TYPE"].ToString();
                    ObjPayUResponse.bank_ref_num = form["bank_ref_num"].ToString();
                    ObjPayUResponse.bankcode = form["bankcode"].ToString();
                    ObjPayUResponse.error = form["error"].ToString();
                    ObjPayUResponse.error_Message = form["error_Message"].ToString();
                    ObjPayUResponse.payuMoneyId = form["payuMoneyId"].ToString();

                    string Msg = ObjPayUResponse.status.ToLower() == "success" ? null : "Fail";
                    string PaymentStatus = accountService.InsertPaymentInformation(ObjPayUResponse, null, 0, null, 0, 0, 0);
                    await accountService.GetOTP(ObjPayUResponse.phone, ObjPayUResponse.amount, ObjPayUResponse.txnid, Msg);
                }
                return null;// RedirectToAction("SellNow");
            }
            catch (Exception ex)
            {
                //Response.Write("<span style='color:red'>" + ex.Message + "</span>");
                //return RedirectToAction("SellNow");
                throw ex;
            }
        }

        [HttpPost]
        public IResponse<ApiResponse> SoldGoods(SoldItems ObjSoldItems)
        {
            PayUResponse ObjPayUResponse = null;
            string TxnUid = null;
            ObjPayUResponse = new PayUResponse();
            TxnUid = accountService.TransactionIdExists(ObjSoldItems.ExistingClientUid, null, 3);
            if (TxnUid != null)
            {
                ObjSoldItems.TxnUid = TxnUid;
                ObjSoldItems.TxnIdExists = 1;
            }
            else
            {
                ObjSoldItems.TxnIdExists = 0;
                TxnUid = accountService.Generatetxnid();
                ObjSoldItems = sellGoodService.SoldGoodsService(ObjSoldItems);
                ObjPayUResponse = new PayUResponse();
                ObjPayUResponse.txnid = TxnUid;
                ObjSoldItems.TxnUid = TxnUid;
                ObjPayUResponse.amount = ObjSoldItems.TotalAmount.ToString();
                string PaymentStatus = accountService.InsertPaymentInformation(ObjPayUResponse, null, 2018, ObjSoldItems.ExistingClientUid, 3, 0, 0);
            }

            ObjSoldItems.ObjClientDetail = sellGoodService.GetClientDetailByUid(ObjSoldItems.ExistingClientUid);
            return null;// context.Stringify(ObjSoldItems);
        }

        [HttpPost]
        public void OpenPayUMoneyForClient(FormCollection FormData)
        {
            SoldItems ObjSoldItems = null;
            Clients ObjClient = null;
            //if (FormData["formcollectionData"] != null && FormData["formcollectionData"] != "")
            //{
            //    ObjSoldItems = context.Parse<SoldItems>(FormData["formcollectionData"]);
            //    if (ObjSoldItems != null && ObjSoldItems.TxnUid != "")
            //    {
            //        if (ObjSoldItems.TxnIdExists == 1)
            //        {
            //            ObjClient = sellGoodService.GetClientDetailByUid(ObjSoldItems.ExistingClientUid);
            //            if (ObjClient != null)
            //            {
            //                ObjSoldItems.Mobile = ObjClient.Mobile;
            //                ObjSoldItems.Email = ObjClient.Email;
            //                ObjSoldItems.PersonName = ObjClient.PersonName;
            //                ObjSoldItems.PayeeCode = 3;
            //            }
            //            else
            //            {
            //                // unable to get parent detail
            //            }
            //        }

            //        string MarchandKey = context.GetWebConfigAppSettingValue("MarchandKey");
            //        string Salt = context.GetWebConfigAppSettingValue("Salt");
            //        string ServiceProvider = context.GetWebConfigAppSettingValue("ServiceProvider");
            //        string PayUSuccessUrl = context.GetWebConfigAppSettingValue("ClientPayUSuccessUrl");
            //        string PayUFailUrl = context.GetWebConfigAppSettingValue("ClientPayUFailUrl");
            //        string PayUBaseUrl = context.GetWebConfigAppSettingValue("PayUBaseUrl");

            //        var firstName = ObjSoldItems.PersonName;
            //        var amount = ObjSoldItems.TotalAmount;
            //        var productInfo = "School fees payment";
            //        var email = ObjSoldItems.Email;
            //        var phone = ObjSoldItems.Mobile;
            //        var key = MarchandKey;
            //        var salt = Salt;

            //        var myremotepost = new RemotePost { Url = PayUBaseUrl + "/_payment" };
            //        myremotepost.Add("key", key);
            //        myremotepost.Add("txnid", ObjSoldItems.TxnUid);
            //        myremotepost.Add("amount", amount.ToString());
            //        myremotepost.Add("productinfo", productInfo);
            //        myremotepost.Add("firstname", firstName);
            //        myremotepost.Add("phone", phone);
            //        myremotepost.Add("email", email);
            //        myremotepost.Add("surl", PayUSuccessUrl);
            //        myremotepost.Add("furl", PayUFailUrl);
            //        myremotepost.Add("service_provider", "payu_paisa");

            //        string hashString = key + "|" + ObjSoldItems.TxnUid + "|" + amount + "|" + productInfo + "|" + firstName + "|" + email + "|||||||||||" + salt;
            //        string hash = accountService.Generatehash512(hashString);
            //        myremotepost.Add("hash", hash);
            //        myremotepost.Post();
            //    }
            //    else
            //    {
            //        RedirectToAction("PayUReturnAction", "Home");
            //    }
            //}
        }

        [HttpPost]
        public void OpenPayUMoney(FormCollection FormData)
        {
            //FeesPaymentDetail ObjFeesPaymentDetail = null;
            //StudentParentDetail ObjStudentParentDetail = null;
            //if (FormData["formcollectionData"] != null && FormData["formcollectionData"] != "")
            //{
            //    ObjFeesPaymentDetail = context.Parse<FeesPaymentDetail>(FormData["formcollectionData"]);
            //    if (ObjFeesPaymentDetail != null && ObjFeesPaymentDetail.TxnUid != "")
            //    {
            //        if (ObjFeesPaymentDetail.TxnIdExists == 1)
            //        {
            //            ObjStudentParentDetail = accountService.GetStudentDetailByUid(ObjFeesPaymentDetail.ExistingClientUid);
            //            if (ObjStudentParentDetail != null)
            //            {
            //                ObjFeesPaymentDetail = accountService.FeesPaymentDataService(ObjFeesPaymentDetail, ObjStudentParentDetail);
            //            }
            //            else
            //            {
            //                // unable to get parent detail
            //            }
            //        }
            //        string MarchandKey = context.GetWebConfigAppSettingValue("MarchandKey");
            //        string Salt = context.GetWebConfigAppSettingValue("Salt");
            //        string ServiceProvider = context.GetWebConfigAppSettingValue("ServiceProvider");
            //        string PayUSuccessUrl = context.GetWebConfigAppSettingValue("ClientPayUSuccessUrl");
            //        string PayUFailUrl = context.GetWebConfigAppSettingValue("ClientPayUFailUrl");
            //        string PayUBaseUrl = context.GetWebConfigAppSettingValue("PayUBaseUrl");

            //        var firstName = ObjFeesPaymentDetail.PersonName;
            //        var amount = ObjFeesPaymentDetail.TotalAmount;
            //        var productInfo = "School fees payment";
            //        var email = ObjFeesPaymentDetail.Email;
            //        var phone = ObjFeesPaymentDetail.Mobile;
            //        var key = MarchandKey;
            //        var salt = Salt;

            //        var myremotepost = new RemotePost { Url = PayUBaseUrl + "/_payment" };
            //        myremotepost.Add("key", key);
            //        myremotepost.Add("txnid", ObjFeesPaymentDetail.TxnUid);
            //        myremotepost.Add("amount", amount.ToString());
            //        myremotepost.Add("productinfo", productInfo);
            //        myremotepost.Add("firstname", firstName);
            //        myremotepost.Add("phone", phone);
            //        myremotepost.Add("email", email);
            //        myremotepost.Add("surl", PayUSuccessUrl);
            //        myremotepost.Add("furl", PayUFailUrl);
            //        myremotepost.Add("service_provider", "payu_paisa");

            //        string hashString = key + "|" + ObjFeesPaymentDetail.TxnUid + "|" + amount + "|" + productInfo + "|" + firstName + "|" + email + "|||||||||||" + salt;
            //        string hash = accountService.Generatehash512(hashString);
            //        myremotepost.Add("hash", hash);
            //        myremotepost.Post();
            //    }
            //    else
            //    {
            //        RedirectToAction("PayUReturnAction", "Home");
            //    }
            //}
        }

        private string GetClientInfo()
        {
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> VerifyNGetPartial(SoldItems ObjSoldItems, string ClientViewName)
        {

            //string View = ClientViewName.ToLower();
            //switch (View)
            //{
            //    case "default":
            //        var Data = SoldGoods(ObjSoldItems);
            //        if (string.IsNullOrEmpty(Data))
            //            ViewBag.PartialViewData = "nil";
            //        else
            //            ViewBag.PartialViewData = Data;
            //        return PartialView("_DefaultBillLayout");
            //    default: return null;
            //}
            return null;
        }

        public class RemotePost
        {
            public readonly System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();

            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                //System.Web.HttpContext.Current.Response.Clear();

                //System.Web.HttpContext.Current.Response.Write("<html><head>");

                //System.Web.HttpContext.Current.Response.Write($"</head><body onload=\"document.{FormName}.submit()\">");
                //System.Web.HttpContext.Current.Response.Write($"<form name=\"{FormName}\" method=\"{Method}\" action=\"{Url}\" >");
                //for (var i = 0; i < Inputs.Keys.Count; i++)
                //{
                //    System.Web.HttpContext.Current.Response.Write($"<input name=\"{Inputs.Keys[i]}\" type=\"hidden\" value=\"{Inputs[Inputs.Keys[i]]}\">");
                //}
                //System.Web.HttpContext.Current.Response.Write("</form>");
                //System.Web.HttpContext.Current.Response.Write("</body></html>");

                //System.Web.HttpContext.Current.Response.End();
            }
        }
    }
}
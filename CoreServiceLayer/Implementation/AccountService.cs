using BottomhalfCore.Annotations;
using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ORMModels;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceLayer.Implementation
{
    [Transient]
    public class AccountService : CurrentUserObject, IAccountService<AccountService>
    {
        private readonly IValidateModalService<ValidateModalService> validateModalService;
        private readonly UserDefineMapping userDefineMapping;
        public readonly IDb db;

        public AccountService(IDb db, ValidateModalService validateModalService, UserDefineMapping userDefineMapping, CurrentSession currentSession)
        {
            this.db = db;
            this.validateModalService = validateModalService;
            this.userDefineMapping = userDefineMapping;
            userDetail = currentSession.CurrentUserDetail;
        }

        public string StaffSalaryService(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            if (SortBy == null || SortBy == "")
                SortBy = " StaffMemberUid";
            return StaffSalaryGenericService(SearchString, SortBy, PageIndex, PageSize, "sp_StaffSalaryDetail_SelByFilter");
        }

        public string StaffSalaryGenericService(string SearchString, string SortBy, string PageIndex, string PageSize, string ProcedureName)
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

        public string SchoolFeesDetailService(string PayeeUid, string ClassDetailUid, int PaymentYear = 0)
        {
            if (PaymentYear <= 1900)
                PaymentYear = DateTime.Now.Year;
            DbParam[] param = new DbParam[]
            {
                new DbParam(PayeeUid, typeof(System.String), "_PayeeUid"),
                new DbParam(ClassDetailUid, typeof(System.String), "_ClassDetailUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TanentId"),
                new DbParam(PaymentYear, typeof(System.Int32), "_Year")
            };

            DataSet ds = db.GetDataset("sp_PaymentDetail_Filter", param);
            return JsonConvert.SerializeObject(ds);
        }

        public string TransactionIdExists(string PayeeUid, string MonthString, int PayeeCode)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(MonthString, typeof(System.String), "_MonthString"),
                new DbParam(PayeeUid, typeof(System.String), "_PayeeUid"),
                new DbParam(PayeeCode, typeof(System.Int32), "_UserCode"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId"),
                new DbParam(DateTime.Now, typeof(System.DateTime), "_AddedOn")
            };

            var ReturnedState = db.ExecuteSingle("sp_CheckExistingRecordIn_PaymentDetailTbl", param, false);
            if (ReturnedState != DBNull.Value && ReturnedState != null)
                return ReturnedState.ToString();
            return null;
        }

        public string AddEditFeesService(CommonFeesModal ObjCommonFeesModal)
        {
            string ResultSet = null;
            if (!string.IsNullOrEmpty(ObjCommonFeesModal.SchoolFeeDetailId))
            {
                ObjCommonFeesModal.AffectedDate = DateTime.Now;
                ObjCommonFeesModal.IsFeeChanged = true;
            }
            else
            {
                ObjCommonFeesModal.SchoolFeeDetailId = null;
                ObjCommonFeesModal.schooltenentId = this.userDetail.schooltenentId;
                ObjCommonFeesModal.FineForPayeeUid = "";
            }

            ServiceResult ObjServiceResult = validateModalService.ValidateModalFieldsService(typeof(CommonFeesModal), ObjCommonFeesModal);
            DbParam[] param = new DbParam[]
            {
                new DbParam(ObjCommonFeesModal.SchoolFeeDetailId, typeof(System.String), "_schoolfeedetailid"),
                new DbParam(ObjCommonFeesModal.schooltenentId, typeof(System.String), "_schooltenentid"),
                new DbParam(ObjCommonFeesModal.Class, typeof(System.String), "_class"),
                new DbParam(ObjCommonFeesModal.FeeCode, typeof(System.Int32), "_feecode"),
                new DbParam(ObjCommonFeesModal.Amount, typeof(System.Double), "_amount"),
                new DbParam(ObjCommonFeesModal.IsFeeChanged, typeof(System.Boolean), "_isfeechanged"),
                new DbParam(ObjCommonFeesModal.NewAmount, typeof(System.Double), "_newamount"),
                new DbParam(ObjCommonFeesModal.AffectedDate, typeof(System.DateTime), "_effecteddate"),
                new DbParam(ObjCommonFeesModal.PaymentDate, typeof(System.Int32), "_paymentDate"),
                new DbParam(ObjCommonFeesModal.LastPaymentDate, typeof(System.String), "_lastPaymentDate"),
                new DbParam(ObjCommonFeesModal.LateFineAmount, typeof(System.Double), "_lateFineAmount"),
                new DbParam(ObjCommonFeesModal.LateFineType, typeof(System.String), "_lateFineType"),
                new DbParam(ObjCommonFeesModal.LateFinePerDayAmount, typeof(System.Double), "_lateFinePerDayAmount"),
                new DbParam(ObjCommonFeesModal.LateFinePerMonthAmount, typeof(System.Double), "_lateFinePerMonthAmount"),
                new DbParam(this.userDetail.UserId, typeof(System.String), "_adminid")
            };

            DataSet ds = db.GetDataset("sp_schoolfeedetails_InsUpd", param);
            if (ds != null && ds.Tables.Count > 0)
                ResultSet = JsonConvert.SerializeObject(ds);
            else
                ResultSet = JsonConvert.SerializeObject(new List<string> { "EMPTY" });
            return ResultSet;
        }

        public string Generatetxnid()
        {
            var rnd = new Random();
            var strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            var txnid1 = strHash.Substring(0, 20);
            return txnid1;
        }

        public string Generatehash512(string text)
        {
            var message = Encoding.UTF8.GetBytes(text);
            var hashString = new SHA512Managed();
            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }

        public StudentParentDetail GetStudentDetailByUid(string StudentUid)
        {
            StudentParentDetail ObjStudentParentDetail = new StudentParentDetail();
            String ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(StudentUid, typeof(System.String), "_StudentUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId")
            };
            DataSet ds = db.GetDataset("sp_ParentDetail_ByStudentUid", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ObjStudentParentDetail = userDefineMapping.ConvertToObject(ds.Tables[0], ObjStudentParentDetail.GetType(), out ResultSet) as StudentParentDetail;
                if (ResultSet != "100")
                    return null;
            }
            return ObjStudentParentDetail;
        }

        public Schoolfeedetails GetFeeDetailByClassDetailUid(string ClassDetailUid)
        {
            Schoolfeedetails ObjSchoolfeedetails = new Schoolfeedetails();
            String ResultSet = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(ClassDetailUid, typeof(System.String), "_classDetailUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentUid")
            };
            DataSet ds = db.GetDataset("sp_GetFeeDetail_ByClassDetailUid", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ObjSchoolfeedetails = userDefineMapping.ConvertToObject(ds.Tables[0], ObjSchoolfeedetails.GetType(), out ResultSet) as Schoolfeedetails;
                if (ResultSet != "100")
                    return null;
            }
            return ObjSchoolfeedetails;
        }

        public FeesPaymentDetail FeesPaymentDataService(FeesPaymentDetail ObjFeesPaymentDetail, StudentParentDetail ObjStudentParentDetail)
        {
            Double TotalAmount = 0.0;
            if (!string.IsNullOrEmpty(ObjFeesPaymentDetail.ExistingClientUid))
            {
                if (ObjStudentParentDetail != null)
                {
                    Schoolfeedetails ObjSchoolfeedetails = GetFeeDetailByClassDetailUid(ObjStudentParentDetail.ClassDetailUid);
                    if (ObjSchoolfeedetails != null)
                    {
                        foreach (var Fees in ObjFeesPaymentDetail.feesDetail)
                        {
                            if (ObjSchoolfeedetails.IsFeeChanged)
                            {
                                if (Fees.ForMonth == Convert.ToDateTime(ObjSchoolfeedetails.AffectedDate).Month - 1)
                                    TotalAmount += ObjSchoolfeedetails.NewAmount;
                                else
                                    TotalAmount += ObjSchoolfeedetails.Amount;
                            }
                            else
                                TotalAmount += ObjSchoolfeedetails.Amount;
                        }
                        ObjFeesPaymentDetail.Mobile = ObjStudentParentDetail.FatherMobileno;
                        ObjFeesPaymentDetail.Email = ObjStudentParentDetail.Fatheremailid;
                        ObjFeesPaymentDetail.PersonName = ObjStudentParentDetail.FatherFirstName + " " + ObjStudentParentDetail.FatherLastName;
                        ObjFeesPaymentDetail.TotalAmount = TotalAmount;
                        ObjFeesPaymentDetail.PayeeCode = 1;
                    }
                }
            }
            return ObjFeesPaymentDetail;
        }

        public string InsertPaymentInformation(PayUResponse ObjPayUResponse, string MonthNumber, int Year, string PayeeUid, int PayeeCode, int FeeCode, int FineCode)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_schooltenentid"),
                new DbParam(PayeeUid, typeof(System.String), "_payeeuid"),
                new DbParam(MonthNumber, typeof(System.String), "_paymentformonth"),
                new DbParam(Year, typeof(System.Int32), "_foryear"),
                new DbParam(PayeeCode, typeof(System.String), "_PayeeCode"),
                new DbParam(FeeCode, typeof(System.Int32), "_feecode"),
                new DbParam(FineCode, typeof(System.Int32), "_finecode"),
                new DbParam(ObjPayUResponse.txnid, typeof(System.String), "_txnid"),
                new DbParam(ObjPayUResponse.status, typeof(System.String), "_status"),
                new DbParam(ObjPayUResponse.unmappedstatus, typeof(System.String), "_unmappedstatus"),
                new DbParam(ObjPayUResponse.mode, typeof(System.String), "_mode"),
                new DbParam(Convert.ToDouble(ObjPayUResponse.amount), typeof(System.Double), "_amountpaid"),
                new DbParam(ObjPayUResponse.field9, typeof(System.String), "_statusmessage"),
                new DbParam(ObjPayUResponse.payuMoneyId, typeof(System.String), "_payuMoneyId"),
                new DbParam(ObjPayUResponse.bankcode, typeof(System.String), "_bankcode"),
                new DbParam(ObjPayUResponse.bank_ref_num, typeof(System.String), "_bankreferenceno"),
                new DbParam(ObjPayUResponse.PG_TYPE, typeof(System.String), "_paymenttype")
            };
            var ReturnedState = db.ExecuteNonQuery("sp_paymentdetail_InsUpd", param, true);
            return ReturnedState;
        }

        public async Task<string> GetOTP(string userNumber, string Amount, string TransactionId, string Status)
        {
            var client = new HttpClient();
            Random rand = new Random();
            string SubMsg = "";
            if (!string.IsNullOrEmpty(Status))
                SubMsg = "Fail to receive amount of Rs. " + Amount;
            else
                SubMsg = "Payment of Rs. " + Amount + " received successfully";
            string uri = "http://newsms.designhost.in/index.php/smsapi/httpapi/?" +
                            "uname=ramalo&password=123456&sender=ISTIYA&receiver=" + userNumber + "&route=TA&msgtype=1&sms=" +
                             SubMsg + ". TransactionId: " + TransactionId + ". For any query call: 9100544384";

            client.BaseAddress = new Uri(uri);
            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return null;
        }
    }
}

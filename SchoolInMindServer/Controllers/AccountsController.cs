using BottomhalfCore.Annotations;
using CommonModal.Models;
using CommonModal.ORMModels;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;
using System.Linq;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    [Route("api/[controller]")]
    public class AccountsController : BaseController
    {
        // GET: Accounts
        private readonly IAccountService<AccountService> accountService;
        public AccountsController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        [Route("FeesDetail")]
        public IResponse<ApiResponse> FeesFilterResult(string PayeeUid, string ClassDetailUid, int PaymentYear)
        {
            string Result = this.accountService.SchoolFeesDetailService(PayeeUid, ClassDetailUid, PaymentYear);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("AddEditFees")]
        public string AddEditFees(CommonFeesModal ObjCommonFeesModal)
        {
            string Status = string.Empty;
            Status = this.accountService.AddEditFeesService(ObjCommonFeesModal);
            return Status;
        }

        [HttpPost]
        [Route("FeesPayment")]
        public string FeesPayment(FeesPaymentDetail ObjFeesPaymentDetail)
        {
            PayUResponse ObjPayUResponse = null;
            string TxnUid = null;
            ObjPayUResponse = new PayUResponse();
            string PaymentForMonths = ObjFeesPaymentDetail.feesDetail.Select(x => x.ForMonth.ToString()).Aggregate((a, b) => a + "," + b);
            TxnUid = this.accountService.TransactionIdExists(ObjFeesPaymentDetail.ExistingClientUid, PaymentForMonths, 1);
            if (TxnUid != null)
            {
                ObjFeesPaymentDetail.TxnUid = TxnUid;
                ObjFeesPaymentDetail.TxnIdExists = 1;
            }
            else
            {
                ObjFeesPaymentDetail.TxnIdExists = 0;
                TxnUid = accountService.Generatetxnid();
                if (ObjFeesPaymentDetail != null && ObjFeesPaymentDetail.feesDetail.Count > 0)
                {
                    StudentParentDetail ObjStudentParentDetail = accountService.GetStudentDetailByUid(ObjFeesPaymentDetail.ExistingClientUid);
                    if (ObjStudentParentDetail != null)
                    {
                        ObjFeesPaymentDetail = accountService.FeesPaymentDataService(ObjFeesPaymentDetail, ObjStudentParentDetail);
                        ObjPayUResponse.txnid = TxnUid;
                        ObjPayUResponse.amount = ObjFeesPaymentDetail.TotalAmount.ToString();
                        int FineCode = ObjFeesPaymentDetail.feesDetail.FirstOrDefault().FineCode;
                        int FeeCode = ObjFeesPaymentDetail.feesDetail.FirstOrDefault().FeeCode;
                        string PaymentStatus = accountService.InsertPaymentInformation(ObjPayUResponse, PaymentForMonths, ObjStudentParentDetail.AccedemicStartYear,
                                                ObjFeesPaymentDetail.ExistingClientUid, ObjFeesPaymentDetail.PayeeCode, FeeCode, FineCode);
                    }
                    else
                    {
                        // unable to get parent detail
                    }
                }
            }

            return JsonConvert.SerializeObject(ObjFeesPaymentDetail);
        }

        public ActionResult Salaries()
        {
            //ViewBag.Data = SalarByFilter(null, null, null, null);
            return null;
        }

        public ActionResult Incomes()
        {
            return null;
        }

        public ActionResult ProfitAndLoss()
        {
            return null;
        }

        public ActionResult GrothRate()
        {
            return null;
        }

        [HttpGet]
        [Route("SalarByFilter")]
        public string SalarByFilter(string SearchString, string SortBy, string PageIndex, string PageSize)
        {
            string Result = accountService.StaffSalaryService(SearchString, SortBy, PageIndex, PageSize);
            return Result;
        }

        public ActionResult Stocks()
        {
            //ViewBag.Result = "";
            return null;
        }

        public ActionResult AddItem()
        {
            return null;
        }
    }
}
using BottomhalfCore.Annotations;
using CommonModal.Models;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class HomeController : ControllerBase
    {
        private readonly ISellGoodService<SellGoodService> ObjSellGoodService;
        private readonly IAttendenceService<AttendenceService> ObjAttendenceService;
        private readonly IAccountService<AccountService> ObjAccountService;
        public HomeController(AttendenceService ObjAttendenceService, AccountService ObjAccountService, SellGoodService ObjSellGoodService)
        {
            this.ObjAttendenceService = ObjAttendenceService;
            this.ObjSellGoodService = ObjSellGoodService;
            this.ObjAccountService = ObjAccountService;
        }

        public IResponse<ApiResponse> Index()
        {
            return null;
        }

        public IResponse<ApiResponse> Dashboard()
        {
            return null;
        }

        public IResponse<ApiResponse> Backend()
        {
            return null;
        }

        public IResponse<ApiResponse> About()
        {
            //ViewBag.Message = "Your application description page.";

            return null;
        }

        public IResponse<ApiResponse> Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> IndividualPaymentDetail(string PayeeUid)
        {
            string Result = "";
            Result = ObjAttendenceService.StudenPaymentDetailService(PayeeUid);
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> GetPartial(string PayeeUid, string View)
        {
            //switch (View)
            //{
            //    case "1":
            //        var Data = IndividualPaymentDetail(PayeeUid);
            //        if (string.IsNullOrEmpty(Data))
            //            ViewBag.PartialViewData = new List<string>();
            //        else
            //            ViewBag.PartialViewData = Data;
            //        var Result = PartialView("StudentPayDetail");
            //        return Result;
            //    default: return null;
            //}
            return null;
        }

        [HttpGet]
        public string PaymentDetail(string ClassDetailUid)
        {
            string Result = ObjAttendenceService.PaymentDetailService(ClassDetailUid);
            return Result;
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
                    string PaymentStatus = ObjAccountService.InsertPaymentInformation(ObjPayUResponse, null, 0, null, 1, 0, 0);
                    await ObjAccountService.GetOTP(ObjPayUResponse.phone, ObjPayUResponse.amount, ObjPayUResponse.txnid, Msg);
                }
                return null;// RedirectToAction("Payments");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*---------------------  This function need to be called after every 24 hours to make Attendence View in database ready till last working day.----------------------------*/
        [HttpGet]
        public IResponse<ApiResponse> CalculateAttendence()
        {
            StringBuilder XmlBuilder = null;
            int AbsentCount = 0;
            IList<AttendenceReporter> ObjAttendenceReporterList = null;
            AttendenceReporter ObjAttendenceReporter = null;
            DateTime? AttendenceReportedDate = null;
            string XmlData = null;
            string Result = null;
            DataSet ds = ObjAttendenceService.AttendeceReportService("sp_ClassDetails_GetUid", null);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ObjAttendenceReporterList = new List<AttendenceReporter>();
                int ClassUid = 0;
                string ClassDetailUid = null;
                while (ClassUid < ds.Tables[0].Rows.Count)
                {
                    ClassDetailUid = null;
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[ClassUid]["ClassDetailId"].ToString()))
                    {
                        ClassDetailUid = ds.Tables[0].Rows[ClassUid]["ClassDetailId"].ToString();
                        DataSet AttendenceSet = ObjAttendenceService.AttendeceReportService("sp_AttendenceAll_ByClassDetailUid", ClassDetailUid);
                        if (AttendenceSet.Tables.Count != 0 && AttendenceSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in AttendenceSet.Tables[0].Rows)
                            {
                                if (dr[6] != DBNull.Value)
                                {
                                    AttendenceReportedDate = Convert.ToDateTime(dr[6]);
                                    var CurrentWorkingMonthStartDay = new DateTime(AttendenceReportedDate.Value.Year, AttendenceReportedDate.Value.Month, 1);
                                    int TotalDays = DateTime.DaysInMonth(CurrentWorkingMonthStartDay.Year, CurrentWorkingMonthStartDay.Month);
                                    int CurrentDays = 1;
                                    while (CurrentDays <= TotalDays)
                                    {
                                        var CurrentWorkingDay = new DateTime(AttendenceReportedDate.Value.Year, AttendenceReportedDate.Value.Month, CurrentDays);
                                        if (CurrentWorkingDay.DayOfWeek.ToString() != "Saturday" && CurrentWorkingDay.DayOfWeek.ToString() != "Sunday")
                                        {
                                            if (Convert.ToInt32(dr[CurrentDays + 6]) == 0)
                                                AbsentCount++;
                                        }
                                        CurrentDays++;
                                    }

                                    if (AbsentCount > 0)
                                    {
                                        ObjAttendenceReporter = new AttendenceReporter();
                                        ObjAttendenceReporter.Year = CurrentWorkingMonthStartDay.Year.ToString();
                                        ObjAttendenceReporter.Month = CurrentWorkingMonthStartDay.Month.ToString();
                                        ObjAttendenceReporter.StudentUid = dr[3].ToString();
                                        ObjAttendenceReporter.TenentId = dr[1].ToString();
                                        ObjAttendenceReporter.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentWorkingMonthStartDay.Month);
                                        ObjAttendenceReporter.TotalAbs = AbsentCount;
                                        ObjAttendenceReporter.ClassDetailUid = ClassDetailUid;
                                        ObjAttendenceReporterList.Add(ObjAttendenceReporter);
                                        AbsentCount = 0;
                                    }
                                }
                            }
                        }
                    }
                    ClassUid++;
                }
            }

            if (ObjAttendenceReporterList != null && ObjAttendenceReporterList.Count() > 0)
            {
                int TotalWorkingDaysTillDate = TotalWorkingDaysTillDateFun(new DateTime(2018, 3, 1), DateTime.Now);
                string PreXmlSample = "<field id=\"AbsentInJanuary\"></field>" +
                                      "<field id=\"AbsentInFebruary\"></field>" +
                                      "<field id =\"AbsentInMarch\"></field>" +
                                      "<field id=\"AbsentInApril\"></field>" +
                                      "<field id=\"AbsentInMay\"></field>" +
                                      "<field id=\"AbsentInJune\"></field>" +
                                      "<field id =\"AbsentInJuly\"></field>" +
                                      "<field id=\"AbsentInAugust\"></field>" +
                                      "<field id=\"AbsentInSeptember\"></field>" +
                                      "<field id=\"AbsentInOctober\"></field>" +
                                      "<field id=\"AbsentInNovember\"></field>" +
                                      "<field id=\"AbsentInDecember\"></field>";

                XmlBuilder = new StringBuilder();
                XmlBuilder.Append("<? xml version=\"1.0\"?>");
                var AttendenceByGroup = ObjAttendenceReporterList.GroupBy(x => x.StudentUid);
                foreach (var Records in AttendenceByGroup)
                {
                    var TotalAbsent = Records.Sum(x => x.TotalAbs);
                    XmlBuilder.Append("<row>");
                    XmlBuilder.Append("<field id=\"StudentUid\">" + Records.FirstOrDefault().StudentUid + "</field>");
                    XmlBuilder.Append("<field id=\"TenentUid\">" + Records.FirstOrDefault().TenentId + "</field>");
                    XmlBuilder.Append("<field id=\"TotalWorkingDays\">" + TotalWorkingDaysTillDate + "</field>");
                    XmlBuilder.Append("<field id=\"TotalAbsent\">" + TotalAbsent + "</field>");
                    XmlBuilder.Append("<field id=\"AppliedLeavesUid\"></field>");
                    XmlBuilder.Append("<field id=\"ClassDetailId\">" + Records.FirstOrDefault().ClassDetailUid + "</field>");
                    string ActualXmlData = PreXmlSample;
                    foreach (var Item in Records)
                    {
                        if (ActualXmlData.IndexOf(Item.MonthName) != -1)
                        {
                            ActualXmlData = ActualXmlData.Replace(Item.MonthName + "\">", Item.MonthName + "\">" + Item.TotalAbs.ToString());
                        }
                    }
                    XmlBuilder.Append(ActualXmlData);
                    XmlBuilder.Append("</row>");
                    ActualXmlData = null;
                }

                XmlData = XmlBuilder.ToString();
                Result = ObjAttendenceService.CalculatedAttendenceBulkUpload(XmlData);
                return null;// Json(new { status = 200 }, JsonRequestBehavior.AllowGet);
            }

            return null;// Json(new { status = 500 }, JsonRequestBehavior.AllowGet);
        }

        public int TotalWorkingDaysTillDateFun(DateTime FromDate, DateTime EffectiveDate)
        {
            int WorkingDays = 0;
            Boolean ReachTillDate = false;
            if ((EffectiveDate - FromDate).Days > 0)
            {
                DateTime WorkingDate = FromDate;
                int WorkingMonth = FromDate.Month;

                for (int i = 0; i < 12; i++)
                {
                    int TotalDays = DateTime.DaysInMonth(WorkingDate.Year, WorkingDate.Month + i);
                    int IndexDay = 1;
                    while (IndexDay <= TotalDays)
                    {
                        var CurrentWorkingDate = new DateTime(WorkingDate.Year, WorkingDate.Month + i, IndexDay);
                        if ((EffectiveDate - CurrentWorkingDate).Days > 0)
                        {
                            if (CurrentWorkingDate.DayOfWeek.ToString() != "Sunday")
                                WorkingDays++;
                        }
                        else
                        {
                            if (CurrentWorkingDate.DayOfWeek.ToString() != "Sunday")
                                WorkingDays++;
                            ReachTillDate = true;
                            break;
                        }
                        IndexDay++;
                    }

                    if ((WorkingMonth + i) == 12)
                        WorkingDate = new DateTime(EffectiveDate.Year, 1, 1);

                    if (ReachTillDate)
                        break;
                }
            }
            return WorkingDays;
        }

        public IResponse<ApiResponse> GetUserObjectSession()
        {
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> GetAllUserSesstionObject()
        {
            string SessionObjectResult = ObjAttendenceService.GetSessionObjectService(null, 1);
            return null;// Json(SessionObjectResult, JsonRequestBehavior.AllowGet);
        }
    }

    public class AttendenceReporter
    {
        public string Year { set; get; }
        public string Month { set; get; }
        public string MonthName { set; get; }
        public int TotalAbs { set; get; }
        public string StudentUid { set; get; }
        public string TenentId { set; get; }
        public string ClassDetailUid { set; get; }
    }
}

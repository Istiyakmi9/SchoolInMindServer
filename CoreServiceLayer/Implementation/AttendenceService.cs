using BottomhalfCore.CacheManagement.Caching;
using BottomhalfCore.CacheManagement.CachingInterface;
using BottomhalfCore.DatabaseLayer.Common.Code;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Newtonsoft.Json;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace CoreServiceLayer.Implementation
{
    public class AttendenceService : CurrentUserObject, IAttendenceService<AttendenceService>
    {
        private ICacheManager<CacheManager> cacheManager;
        private readonly IDb db;
        private readonly ICommonService<CommonService> commonService;
        public AttendenceService(CommonService commonService)
        {
            this.commonService = commonService;
            this.cacheManager = CacheManager.GetInstance();
            userDetail = cacheManager.Get("userdetail") as UserDetail;
        }

        public string GetAttendenceRecordService(AttendenceReport objAttendenceReport)
        {
            AttendenceApiResult objAttendenceApiResult = null;
            List<string> objAbsentOn = null;
            string outcome = null;
            List<string> objAbsentDate = null;
            DateTime ProcedureDate = DateTime.Now;
            if (objAttendenceReport.AttendenceDate != null)
                ProcedureDate = Convert.ToDateTime(objAttendenceReport.AttendenceDate);
            DbParam[] param = new DbParam[]
            {
                new DbParam(ProcedureDate, typeof(System.DateTime), "_attendenceDate"),
                new DbParam(objAttendenceReport.Class, typeof(System.String), "_Class"),
                new DbParam(objAttendenceReport.StudentUid, typeof(System.String), "_studentuid"),
                new DbParam(objAttendenceReport.SchoolTenentId, typeof(System.String), "_schoolTenentId")
            };
            DataSet ds = db.GetDataset("sp_studattendencereport_bymonth", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int len = ds.Tables[0].Columns.Count;
                objAbsentOn = new List<string>();
                objAbsentDate = new List<string>();
                objAttendenceApiResult = new AttendenceApiResult();
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                foreach (DataRow result in ds.Tables[0].Rows)
                {
                    for (var i = 7; i < len; i++)
                    {
                        if (result[i] != DBNull.Value)
                        {
                            if (result[i].ToString() == "0")
                            {
                                objAbsentOn.Add((i - 6).ToString());
                                objAbsentDate.Add(startDate.AddDays((i - 6)).ToString());
                            }
                        }

                    }
                }

                objAttendenceApiResult.absentOn = objAbsentOn;
                objAttendenceApiResult.absentDate = objAbsentDate;
                objAttendenceApiResult.yearAverage = 80;
                objAttendenceApiResult.currentMonthAverage = 84;
                outcome = JsonConvert.SerializeObject(objAttendenceApiResult);
            }
            return outcome;
        }

        public string AttendenceReportByFilterService(AttendenceFilter ObjAttendenceFilter)
        {
            string outcome = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(ObjAttendenceFilter.StartDate, typeof(System.DateTime), "_startDate"),
                new DbParam(ObjAttendenceFilter.EndDate, typeof(System.DateTime), "_endDate"),
                new DbParam(ObjAttendenceFilter.StudentUid, typeof(System.String), "_studentUid"),
                new DbParam(ObjAttendenceFilter.ForClass, typeof(System.String), "_forClass"),
                new DbParam(ObjAttendenceFilter.Section, typeof(System.String), "_section")
            };
            DataSet ds = db.GetDataset("sp_attendenceByFilter", param);
            var ResultedAttendenceData = CreateAttdenceRowSet(ds, null, null);
            outcome = JsonConvert.SerializeObject(ResultedAttendenceData);
            return outcome;
        }

        private List<AttendenceGenerateReport> CreateAttdenceRowSet(DataSet ds, string FromDate, string ToDate)
        {
            List<AttendenceGenerateReport> objAttendenceReportList = null;
            string FromMonth = null;
            int TotalAbsent = 0;
            int TotalDays = 0;
            string ToMonth = null;
            if (FromDate != null && ToDate != null)
            {
                FromMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(FromDate).Month);
                ToMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(ToDate).Month);
            }
            if (userDetail != null)
            {
                AttendenceGenerateReport objAttendenceReport = null;
                IList<string> AbsentOn = null;
                IDictionary<string, Double> MonthlyAttrPercentage = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objAttendenceReportList = new List<AttendenceGenerateReport>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        objAttendenceReport = new AttendenceGenerateReport();
                        if (dr["AttendenceId"] != DBNull.Value)
                            objAttendenceReport.AttendenceId = dr["AttendenceId"].ToString();
                        else
                            objAttendenceReport.AttendenceId = null;

                        if (dr["studentUid"] != DBNull.Value)
                            objAttendenceReport.studentUid = dr["studentUid"].ToString();
                        else
                            objAttendenceReport.studentUid = null;

                        if (dr["SName"] != DBNull.Value)
                            objAttendenceReport.StudenName = dr["SName"].ToString();
                        else
                            objAttendenceReport.StudenName = "NA";

                        if (dr["AttendenceDate"] != DBNull.Value)
                            objAttendenceReport.AttendenceDate = dr["AttendenceDate"].ToString();
                        else
                            objAttendenceReport.AttendenceDate = "NA";

                        AbsentOn = new List<string>();
                        for (int i = 1; i <= 31; i++)
                        {
                            if (dr["Day" + i] != DBNull.Value)
                                AbsentOn.Add(dr["Day" + i].ToString());
                        }

                        if (ds.Tables.Count > 1)
                        {
                            //string MonthName = null;
                            int WorkingYear = userDetail.AccedemicStartYear;
                            MonthlyAttrPercentage = new Dictionary<string, Double>();

                            TotalDays = 0;
                            TotalDays += TotalWorkingDaysInMonth(ds.Tables[1], CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(FromDate).Month));
                            TotalDays += TotalWorkingDaysInMonth(ds.Tables[1], CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(ToDate).Month));

                            //int MonthValue = 0;
                            //for (int MonthIndex = 1; MonthIndex <= 12; MonthIndex++)
                            //{
                            //    if (MonthIndex <= DateTime.Now.Month)
                            //    {
                            //        if (MonthIndex > 10)
                            //            MonthValue = MonthIndex - 12;
                            //        else
                            //            MonthValue = MonthIndex;
                            //        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthValue + 2);
                            //        //if (dr["AbsentIn" + MonthName] != DBNull.Value)
                            //        //{
                            //        //    int Days = TotalWorkingDaysInMonth(ds.Tables[1], MonthName);
                            //        //    Double MonthPercent = 100 - ((Convert.ToDouble(dr["AbsentIn" + MonthName]) * 100) / Days);
                            //        //    MonthlyAttrPercentage.Add(MonthName, MonthPercent);
                            //        //    if ((MonthIndex + 2) == 12)
                            //        //        WorkingYear++;
                            //        //}

                            //        if (MonthName == FromMonth)
                            //            TotalAbsent = Convert.ToInt32(dr["AbsentIn" + MonthName]);

                            //        if (MonthName == ToMonth)
                            //            TotalAbsent += Convert.ToInt32(dr["AbsentIn" + MonthName]);
                            //    }
                            //    else
                            //    {
                            //        break;
                            //    }
                            //}
                        }

                        objAttendenceReport.TotalAbsentTillDate = ((TotalDays - TotalAbsent) * 100) / TotalDays;
                        objAttendenceReport.TotalWorkingDaysTillDate = TotalDays;
                        objAttendenceReport.AbsentOn = AbsentOn;
                        objAttendenceReport.MonthlyPercentage = MonthlyAttrPercentage;
                        objAttendenceReportList.Add(objAttendenceReport);
                    }
                }
            }
            return objAttendenceReportList;
        }

        private int TotalWorkingDaysInMonth(DataTable table, string MonthName)
        {
            int TotalDays = 0;
            foreach (DataRow CalendarRow in table.Rows)
            {
                if (CalendarRow["MonthName"] != DBNull.Value)
                {
                    if (CalendarRow["MonthName"].ToString() == MonthName)
                    {
                        TotalDays = Convert.ToInt32(CalendarRow["NoofWorkingDays"]);
                        break;
                    }
                }
            }
            return TotalDays;
        }

        public string ClassAttendenceRepost(string FromDate, string ToDate, string ClassDetailUid)
        {
            List<AttendenceGenerateReport> objAttendenceReportList = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(FromDate, typeof(System.DateTime), "_fromMonth"),
                new DbParam(ToDate, typeof(System.DateTime), "_toMonth"),
                new DbParam(ClassDetailUid, typeof(System.String), "_classDetailId"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentUid")
            };
            DataSet ds = db.GetDataset("sp_attendencereport_allStudByDate", param);
            objAttendenceReportList = CreateAttdenceRowSet(ds, FromDate, ToDate);
            return JsonConvert.SerializeObject(objAttendenceReportList);
        }

        public string AttendenceAllByFilterService(SearchModal searchModal, string ClassSectionFilter)
        {
            string Result = null;
            this.commonService.ValidateFilterModal(searchModal, "Rollno");
            DbParam[] param = new DbParam[]
            {
                new DbParam(searchModal.SearchString, typeof(System.String), "_searchString"),
                new DbParam(searchModal.SortBy, typeof(System.String), "_sortBy"),
                new DbParam(searchModal.PageIndex, typeof(System.String), "_pageIndex"),
                new DbParam(searchModal.PageSize, typeof(System.String), "_pageSize"),
                new DbParam(ClassSectionFilter, typeof(System.String), "_ClassSectionFilter"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentId")
            };
            DataSet ds = db.GetDataset("sp_Attendence_FullReportFilter", param);
            if (ds.Tables.Count == 2)
                Result = JsonConvert.SerializeObject(ds);
            return Result;
        }

        public string PaymentDetailService(string ClassDetailUid)
        {
            string Result = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(ClassDetailUid, typeof(System.String), "_ClassDetailId"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId")
            };
            DataSet ds = db.GetDataset("sp_StudentDetail_PaymentInfo", param);
            Result = JsonConvert.SerializeObject(ds);
            return Result;
        }

        public string StudenPaymentDetailService(string PayeeUid)
        {
            string Result = null;
            DbParam[] param = new DbParam[]
            {
                new DbParam(PayeeUid, typeof(System.String), "_PayeeUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_TenentId"),
                new DbParam(userDetail.AccedemicStartYear, typeof(System.Int32), "_ForYear"),
                new DbParam(1, typeof(System.Int32), "_UserCode")
            };
            DataSet ds = db.GetDataset("sp_StudentPaymentDetail_Sel", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                Result = JsonConvert.SerializeObject(ds);
            else
                Result = JsonConvert.SerializeObject("No record found");
            return Result;
        }

        public DataSet AttendeceReportService(string ProcedureName, string ClassDetailUid)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(ClassDetailUid, typeof(System.String), "_classDetailUid"),
                new DbParam(userDetail.schooltenentId, typeof(System.String), "_tenentUid")
            };
            DataSet ds = db.GetDataset(ProcedureName, param);
            return ds;
        }

        public string CalculatedAttendenceBulkUpload(string XmlData)
        {
            DbParam[] param = new DbParam[]
            {
                new DbParam(XmlData, typeof(System.String), "_xmlData")
            };
            var Result = db.ExecuteNonQuery("sp_AttendenceConsolidateReport_InsertBulkByXml", param, true);
            return Result;
        }

        public string GetSessionObjectService(string Token, int Days)
        {
            string State = string.Empty;
            DbParam[] param = new DbParam[]
            {
                new DbParam(Token, typeof(System.String), "_Token"),
                new DbParam(Days, typeof(System.Int32), "_Days")
            };

            var OutCome = db.GetDataset("sp_TrackSessionObject_Sel", param);
            if (OutCome != null)
                State = JsonConvert.SerializeObject(OutCome);
            return State;
        }

        public class AttendenceApiResult
        {
            public List<string> absentDate { set; get; }
            public List<string> absentOn { set; get; }
            public int yearAverage { set; get; }
            public int currentMonthAverage { set; get; }
        }
    }
}

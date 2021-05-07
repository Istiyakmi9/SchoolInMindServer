using CommonModal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IAttendenceService<T>
    {
        string GetAttendenceRecordService(AttendenceReport objAttendenceReport);
        string ClassAttendenceRepost(string FromDate, string ToDate, string Class);
        string AttendenceReportByFilterService(AttendenceFilter ObjAttendenceFilter);
        string AttendenceAllByFilterService(SearchModal searchModal, string ClassSectionFilter);
        DataSet AttendeceReportService(string ProcedureName, string ClassDetailUid);
        string CalculatedAttendenceBulkUpload(string XmlData);
        string PaymentDetailService(string ClassDetailUid);
        string StudenPaymentDetailService(string PayeeUid);
        string GetSessionObjectService(string Token, int Days);
    }

    public class AttendenceReport
    {
        public DateTime? AttendenceDate { set; get; }
        public string StudentUid { set; get; }
        public string SchoolTenentId { set; get; }
        public string Class { set; get; }
    }

    public class AttendenceFilter
    {
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public string StudentUid { set; get; }
        public string ForClass { set; get; }
        public string Section { set; get; }
    }
}

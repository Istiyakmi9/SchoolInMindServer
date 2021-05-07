using CommonModal.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IExamResultService<T>
    {
        string ShowExamResult(string StudentId, string TenentId);
        string GetResultById(int ExamId, string StudentId);
        string GetResultByRegistrationNo(ExamResultPostData objExamResultPostData);
        string ViewResultDataService(string SearchStr, string ShortBy, int PageIndex, int PageSize);

        string AddEditResultService(ExamResult ObjExamResult);
    }

    public class ExamResultPostData
    {
        public int? ExamId { set; get; }
        public string RegistrationNo { set; get; }
        public string AcedimicYearFrom { set; get; }
        public string TenentId { set; get; }
    }

    public class ShowResultData
    {
        public string ExamName { set; get; }
        public int AcademicYearFrom { set; get; }
        public int AcademicYearTo { set; get; }
        public string ExamStartDate { set; get; }
        public int ResultPercentage { set; get; }
        public int ExamId { set; get; }
    }
}

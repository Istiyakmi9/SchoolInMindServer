using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class ExamDates
    {
        public string ExamName { set; get; }
        public int ExamId { set; get; }
        public string ClassDetailUid { set; get; }
        public string ExamDetailId { set; get; }
        public string SubjectId { set; get; }
        public DateTime? ExamStartDate { set; get; }
        public DateTime? ExamEndDate { set; get; }
        public int Duration { set; get; }
        public string ExamTime { set; get; }
        public int AcademicYearFrom { set; get; }
        public int AcademicYearTo { set; get; }
    }
}

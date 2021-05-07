using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.ORMModels
{
    public class ExamResult
    {
        public string ExamResultId { set; get; }
        public string TenentUid { set; get; }
        public string ExamDescriptionId { set; get; }
        public string StudentUid { set; get; }
        public string SubjectUid { set; get; }
        public float Marks { set; get; }
        public string Grade { set; get; }
        public int AcedemicYearFrom { set; get; }
    }
}

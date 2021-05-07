using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class Marks
    {
        public long MarksUid { set; get; }
        public string StudentUid { set; get; }
        public int MarksObtain { set; get; }
        public long GradeObtain { set; get; }
        public string SubjectUid { set; get; }
    }
}

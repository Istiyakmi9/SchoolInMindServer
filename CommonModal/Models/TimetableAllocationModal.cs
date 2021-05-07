using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class TimetableAllocationModal
    {
        public string TimetableUid { set; get; }
        public string ClassDetailUid { set; get; }
        public string RulebookUid { set; get; }
        public string SubstitutedFacultiUid { set; get; }
        public string FacultyUid { set; get; }
        public string SubjectUid { set; get; }
        public string SubjectName { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public int Period { set; get; }
        public int WeekDayNum { set; get; }
    }
}

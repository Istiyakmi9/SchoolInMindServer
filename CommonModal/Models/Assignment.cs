using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class Assignment
    {
        public string Class { set; get; }
        public string ClassDetailUid { set; get; }
        public string FacultyUid { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public DateTime SubmissionDate { set; get; }
    }
}

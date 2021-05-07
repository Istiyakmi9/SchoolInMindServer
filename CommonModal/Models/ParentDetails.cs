using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class ParentDetails
    {
        public string FatherFirstName { set; get; }
        public string FatherLastName { set; get; }
        public string Fatheroccupation { set; get; }
        public string FatherMobileno { set; get; }
        public string Fatherqualification { set; get; }
        public string Fatheremailid { set; get; }
        public string LocalGuardianFullName { set; get; }
        public string LocalGuardianMobileno { set; get; }
        public string LocalGuardianemailid { set; get; }
        public string MotherFirstName { set; get; }
        public string MotherLastName { set; get; }
        public string MotherMobileno { set; get; }
        public string MotherOccupation { set; get; }
        public string MotherQualification { set; get; }
        public string Motheremailid { set; get; }
        public string ParentDetailId { set; get; }
    }

    public class Faculty { }
    public class Student { }
    public class NoticeBoard { }
    public class Grade { }
}

using System;

namespace CommonModal.ORMModels
{
    public class StudentParentDetail
    {
        public string StudentUid { set; get; }
        public string ClassDetailUid { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string ImageUrl { set; get; }
        public int AccedemicStartYear { set; get; }
        public int? rollno { set; get; }
        public string registrationNo { set; get; }
        public string ParentDetailId { set; get; }
        public string SchooltenentId { set; get; }
        public string FatherFirstName { set; get; }
        public string FatherLastName { set; get; }
        public string MotherFirstName { set; get; }
        public string MotherLastName { set; get; }
        public string LocalGuardianFullName { set; get; }
        public string FatherMobileno { set; get; }
        public string MotherMobileno { set; get; }
        public string LocalGuardianMobileno { set; get; }
        public string FullAddress { set; get; }
        public string Fatheremailid { set; get; }
        public string Motheremailid { set; get; }
        public string LocalGuardianemailid { set; get; }
        public string FatherOccupation { set; get; }
        public DateTime CreatedOn { set; get; }
        public DateTime? UpdatedOn { set; get; }
        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
    }
}

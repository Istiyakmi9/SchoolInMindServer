using BottomhalfCore.Annotations;
using System;

namespace CommonModal.ORMModels
{
    public class Facultydetails
    {
        public string FacultyUid { set; get; }
        public string StaffMemberUid { set; get; }
        public string SchooltenentId { set; get; }
        public string ClassTeacherForClass { set; get; }
        public string Class { set; get; }
        public string Section { set; get; }
        [Required]
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Gender { set; get; }
        public string Dob { set; get; }
        public string Doj { set; get; }
        public string MobileNumber { set; get; }
        public string AlternetNumber { set; get; }
        public string ImageUrl { set; get; }
        public string Email { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Pincode { set; get; }
        public string Subjects { set; get; }
        public string Type { set; get; }
        public string University { set; get; }
        public string DegreeType { set; get; }
        public string Grade { set; get; }
        public string Marks { set; get; }
        public string YearExprience { set; get; }
        public string MonthExprience { set; get; }
        public string QualificationId { set; get; }
        public string DesignationId { set; get; }
        public DateTime CreatedOn { set; get; }
        public DateTime? UpdatedOn { set; get; }
        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace CommonModal.Models
{
    public class QualificationDetail
    {
        public string SchoolUniversityName { set; get; }
        public string DegreeName { set; get; }
        public string Grade { set; get; }
        public string Position { set; get; }
        public float MarksObtain { set; get; }
        public string Title { set; get; }
        public string ProofOfDocumentationPath { set; get; }
        public int ExprienceInYear { set; get; }
        public int ExperienceInMonth { set; get; }
    }
    public class RegistrationFormData : QualificationDetail
    {
        [Required]
        public string FirstName { set; get; }
        public string AccessLevelUid { set; get; }
        public string LastName { set; get; }
        public bool Gender { set; get; }
        public DateTime Dob { set; get; }
        [Required]
        public string MobileNumber { set; get; }
        public string StaffMemberUid { set; get; }
        public string AlternetNumber { set; get; }
        public string Email { set; get; }
        public string Address { set; get; }
        public string State { set; get; }
        public string City { set; get; }
        public int Pincode { set; get; }
        public string University { set; get; }
        public float Marks { set; get; }
        public string AdminId { set; get; }
        public string SchoolTenentId { set; get; }
        public string ClassDetailUid { set; get; }
        public string ImageUrl { set; get; }
        public string Designation { set; get; }
        public string Subjects { set; get; }
        public string Type { set; get; }
        public string ProfileImageName { set; get; }
        public int NumberOfDocuments { set; get; }
        public string ExistingDocumentFileName { set; get; }
        public bool IsQuickRegistration { set; get; }
        public string ClassTeacherForClass { set; get; }
        public string Doj { set; get; }
        public string QualificationId { set; get; }
        public string DesignationId { set; get; }
        public string Class { set; get; }
        public string Section { set; get; }
        public string ApplicationFor { set; get; }
    }
}

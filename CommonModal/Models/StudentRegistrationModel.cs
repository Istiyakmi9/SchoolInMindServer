using System;
using System.ComponentModel.DataAnnotations;

namespace CommonModal.Models
{
    public class StudentRegistrationModel
    {
        public string StudentUid { set; get; }
        public string SchooltenentId { set; get; }
        public string ParentDetailId { set; get; }
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }
        public string ImageUrl { set; get; }
        public DateTime? Dob { set; get; }
        public int Age { set; get; } = 0;
        public bool Sex { set; get; } = true;
        public string LastSchoolAddress { set; get; }
        public string LastSchoolName { set; get; }
        public string LastSchoolMedium { set; get; }
        public string CurrentSchoolMedium { set; get; }
        public int? Rollno { set; get; } = null;
        public string Mobilenumber { set; get; }
        public string AlternetNumber { set; get; }
        public string EmailId { set; get; }
        public string RegistrationNo { set; get; }
        public DateTime AdmissionDatetime { set; get; }
        public int? FeeCode { set; get; } = 0;
        public string MotherTongue { set; get; }
        public string Religion { set; get; }
        public string Catagory { set; get; }
        public string CatagoryDocPath { set; get; }
        public string SiblingRegistrationNo { set; get; }
        public string LastClass { set; get; }

        // Parent detail below
        [Required]
        public string FatherFirstName { set; get; }
        public string FatherLastName { set; get; }
        public string MotherFirstName { set; get; }
        public string MotherLastName { set; get; }
        public string LocalGuardianFullName { set; get; }
        [Required]
        public string FatherMobileno { set; get; }
        public string MotherMobileno { set; get; }
        public string LocalGuardianMobileno { set; get; }
        public string FullAddress { set; get; }
        public string City { set; get; }
        public string Pincode { set; get; }
        public string State { set; get; }
        public string Fatheremailid { set; get; }
        public string Motheremailid { set; get; }
        public string LocalGuardianemailid { set; get; }
        public string Fatheroccupation { set; get; }
        public string Motheroccupation { set; get; }
        public string Fatherqualification { set; get; }
        public string Motherqualification { set; get; }
        public string Class { set; get; }
        public string Section { set; get; }
        public string ExistingNumber { set; get; }
        public string CreatedBy { set; get; }
        public bool ParentRecordExist { set; get; } = false;
        public string ClassDetailUid { set; get; }
        public string MobileNumbers { set; get; }
        public string EmailIds { set; get; }
        public bool IsQuickRegistration { set; get; } = false;
        public string ProfileImageName { set; get; }
    }
}

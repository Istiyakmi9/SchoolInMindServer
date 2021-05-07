using System;

namespace CommonModal.ORMModels
{
	public class Studentdetail
	{
		public string StudentUid { set; get; }
		public string SchooltenentId { set; get; }
		public string ParentDetailId { set; get; }
		public string ClassDetailUid { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
		public string ImageUrl { set; get; }
		public DateTime? Dob { set; get; }
		public int? Age { set; get; }
		public bool? Sex { set; get; }
		public string Address { set; get; }
		public string City { set; get; }
		public System.Int64? Pincode { set; get; }
		public string State { set; get; }
		public int? Rollno { set; get; }
		public string Mobilenumber { set; get; }
		public string AlternetNumber { set; get; }
		public string EmailId { set; get; }
		public string RegistrationNo { set; get; }
		public DateTime? AdmissionDatetime { set; get; }
		public int? FeeCode { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

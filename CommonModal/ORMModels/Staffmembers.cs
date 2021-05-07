using System;

namespace CommonModal.ORMModels
{
	public class Staffmembers
	{
		public string StaffMemberUid { set; get; }
		public string SchooltenentId { set; get; }
		public string ClassTeacherForClass { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
		public bool? Gender { set; get; }
		public DateTime? Dob { set; get; }
		public DateTime? Doj { set; get; }
		public string MobileNumber { set; get; }
		public string AlternetNumber { set; get; }
		public string ImageUrl { set; get; }
		public string Email { set; get; }
		public string Address { set; get; }
		public string City { set; get; }
		public string State { set; get; }
		public System.Int64? Pincode { set; get; }
		public string QualificationId { set; get; }
		public int? DesignationId { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

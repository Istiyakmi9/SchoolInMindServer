using System;

namespace CommonModal.ORMModels
{
	public class Parentdetail
	{
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

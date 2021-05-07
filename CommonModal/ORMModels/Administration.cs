using System;

namespace CommonModal.ORMModels
{
	public class Administration
	{
		public string AdminId { set; get; }
		public string SchooltenentId { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
		public string Mobile { set; get; }
		public string Alternetno { set; get; }
		public string Email { set; get; }
		public string Address { set; get; }
		public string State { set; get; }
		public string City { set; get; }
		public System.Int64? Pincode { set; get; }
		public string AccessLevelId { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
	}
}

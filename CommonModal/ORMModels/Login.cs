using System;

namespace CommonModal.ORMModels
{
	public class Login
	{
		public string LoginId { set; get; }
		public string UserId { set; get; }
		public string SchooltenentId { set; get; }
		public string MobileNo { set; get; }
		public string Email { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
		public string UserPassword { set; get; }
		public bool? IsVerified { set; get; }
		public bool? IsActive { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
		public bool? IsAdmin { set; get; }
	}
}

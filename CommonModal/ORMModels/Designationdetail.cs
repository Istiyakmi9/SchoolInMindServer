using System;

namespace CommonModal.ORMModels
{
	public class Designationdetail
	{
		public int DesignationId { set; get; }
		public string Description { set; get; }
		public string SchooltenentId { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

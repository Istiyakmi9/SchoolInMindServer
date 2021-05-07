using System;

namespace CommonModal.ORMModels
{
	public class Classdetaildefination
	{
		public string ClassDetailDefinationId { set; get; }
		public string SchooltenentId { set; get; }
		public string Class { set; get; }
		public string ClassDefination { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

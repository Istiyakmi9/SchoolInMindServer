using System;

namespace CommonModal.ORMModels
{
	public class Accesslevel
	{
		public string AccessLevelId { set; get; }
		public string SchooltenentId { set; get; }
		public int AccessCode { set; get; }
		public string Roles { set; get; }
		public string AccessCodeDefination { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
	}
}

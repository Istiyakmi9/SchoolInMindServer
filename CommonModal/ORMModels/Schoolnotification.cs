using System;

namespace CommonModal.ORMModels
{
	public class Schoolnotification
	{
		public string SchoolNotificationId { set; get; }
		public string NotificationHeadline { set; get; }
		public string NotificationDetail { set; get; }
		public string SchooltenentId { set; get; }
		public bool? IsForSchool { set; get; }
		public bool? ForSingleStudent { set; get; }
		public string StudentUid { set; get; }
		public string Class { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

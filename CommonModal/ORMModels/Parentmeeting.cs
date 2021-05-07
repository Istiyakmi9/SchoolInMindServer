using System;

namespace CommonModal.ORMModels
{
	public class Parentmeeting
	{
		public string ParentMeetingId { set; get; }
		public string SchooltenentId { set; get; }
		public string StudentUid { set; get; }
		public string MeetingForClass { set; get; }
		public string MeetingAgenda { set; get; }
		public DateTime MeetingDatetime { set; get; }
		public string Place { set; get; }
		public bool? IsGroupMeeting { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

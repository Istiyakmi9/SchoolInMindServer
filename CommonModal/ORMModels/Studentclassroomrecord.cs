using System;

namespace CommonModal.ORMModels
{
	public class Studentclassroomrecord
	{
		public string StudentClassroomRecordId { set; get; }
		public string SujectId { set; get; }
		public DateTime RecordDateTime { set; get; }
		public string SchooltenentId { set; get; }
		public string StudentUid { set; get; }
		public int? HomeworkSubmitted { set; get; }
		public string PendingWorkDescription { set; get; }
		public string MsgToParent { set; get; }
		public bool? IsConplaint { set; get; }
		public string TeacherUid { set; get; }
		public bool? IsVerifiedByTeacher { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

using System;

namespace CommonModal.ORMModels
{
	public class Classestimetable
	{
		public string ClassesTimetableId { set; get; }
		public string ClassDetailUid { set; get; }
		public string SubjectCodes { set; get; }
		public string TeacherCode { set; get; }
		public string ClassOnDay { set; get; }
		public dynamic StartTime { set; get; }
		public float Duration { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

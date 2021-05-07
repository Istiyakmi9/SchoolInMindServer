using System;

namespace CommonModal.ORMModels
{
	public class Subjects
	{
		public string SubjectId { set; get; }
		public string SchooltenentId { set; get; }
		public string StaffMemberUid { set; get; }
		public string ForClass { set; get; }
		public dynamic Section { set; get; }
		public string SubjectName { set; get; }
		public float SubjectCode { set; get; }
	}
}

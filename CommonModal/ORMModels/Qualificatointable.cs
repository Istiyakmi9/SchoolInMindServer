using System;

namespace CommonModal.ORMModels
{
	public class Qualificatointable
	{
		public string QualificatoinId { set; get; }
		public string SchooltenentId { set; get; }
		public string DegreeName { set; get; }
		public string Grade { set; get; }
		public string Position { set; get; }
		public float? MarksObtain { set; get; }
		public string Title { set; get; }
		public string SchoolUniversityName { set; get; }
		public string ProofOfDocumentationPath { set; get; }
		public int? ExprienceInYear { set; get; }
		public int? ExperienceInMonth { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

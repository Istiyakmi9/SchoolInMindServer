using System;
using System.ComponentModel.DataAnnotations;

namespace CommonModal.ORMModels
{
    public class SchoolCalendar
	{
		public string SchoolCalendarId { set; get; }
        [Required]
		public string Title { set; get; }
		public string Description { set; get; }
		public DateTime StartDate { set; get; }
		public DateTime EndDate { set; get; }
		public string StartTime { set; get; }
		public string EndTime { set; get; }
        [Required]
        public bool IsFullDayEvent { set; get; } = false;
		public string BindUrl { set; get; }
		public DateTime CreatedOn { set; get; }
		public DateTime? UpdatedOn { set; get; }
		public string CreatedBy { set; get; }
		public string UpdatedBy { set; get; }
	}
}

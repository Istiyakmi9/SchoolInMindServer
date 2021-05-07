using System;

namespace CommonModal.ORMModels
{
    public class Announcement
    {
        public string AnnouncementId { set; get; }
        public string Title { set; get; }
        public string AnnouncementMessage { set; get; }
        public string SchooltenentId { set; get; }
        public string ClassDetailUid { set; get; }
        public DateTime CreatedOn { set; get; }
        public DateTime? UpdatedOn { set; get; }
        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
    }
}

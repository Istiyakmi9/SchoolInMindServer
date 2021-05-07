using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class Announcement
    {
        public string AnnouncementId { set; get; }
        public string Title { set; get; }
        public string AnnouncementMessage { set; get; }
        public string ClassDetailId { set; get; }
    }

    public class FetchAnnouncement
    {
        public SearchModal searchModal { set; get; }
        public string ClassDetailUid { set; get; }
        public string StudentUid { set; get; }
    }
}

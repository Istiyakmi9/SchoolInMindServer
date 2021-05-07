using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class Notice
    {
        public string SchoolNotificationId { set; get; }
        public string NotificationHeadline { set; get; }
        public string NotificationDetail { set; get; }
        public string schooltenentId { set; get; }
        public bool IsForSchool { set; get; }
        public bool ForSingleStudent { set; get; }
        public string StudentUid { set; get; }
        public string Class { set; get; }
    }
}

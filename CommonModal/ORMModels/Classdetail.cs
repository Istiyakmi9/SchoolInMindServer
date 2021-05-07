using System;

namespace CommonModal.ORMModels
{
    public class Classdetail
    {
        public string ClassDetailUid { set; get; }
        public string Class { set; get; }
        public int? TotalSeats { set; get; }
        public long RoomUid { set; get; }
        public string Section { set; get; }
        public int? GirlSeats { set; get; }
        public int? BoySeats { set; get; }
        public string SchooltenentId { set; get; }
        public DateTime CreatedOn { set; get; }
        public DateTime? UpdatedOn { set; get; }
        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class Holidays
    {
        public string CalendarHolidayUid { set; get; }
        public string TenentUid { set; get; }
        public string OccasionName { set; get; }
        public string NoOfDays { set; get; }
        public string StartDate { set; get; }
        public string EndDate { set; get; }
    }
}

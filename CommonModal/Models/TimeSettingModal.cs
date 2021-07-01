using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class TimeSettingModal
    {
        public string SchoolStartTime { set; get; }
        public int TotalPeriods { set; get; }
        public int PeriodDurationInMinutes { set; get; }
        public int LunchAfterPeriod { set; get; }
        public List<RuleBook> RuleBookDetail { set; get; }
        public string LunchTime { set; get; }
        public string TimingDescription { set; get; }
        public string LunchDuration { set; get; }
        public string SchoolOtherDetailUid { set; get; }
        public bool IsUpdate { set; get; }
        public List<TimingModal> TimingDetails { set; get; }
    }

    public class RuleBook
    {
        public string RulebookUid { set; get; }
        public int RuleCode { set; get; }
        public string RuleName { set; get; }
    }

    public class TimingModal
    {
        public string TimingDetailUid { set; get; }
        public string RulebookUid { set; get; }
        public string TimingFor { set; get; }
        public int DurationInHrs { set; get; }
        public int DurationInMin { set; get; }
        public string AdminId { set; get; }
    }
}

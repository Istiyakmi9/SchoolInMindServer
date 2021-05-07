using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class AttendenceGenerateReport
    {
        public string AttendenceId { set; get; }
        public string studentUid { set; get; }
        public string StudenName { set; get; }
        public string AttendenceDate { set; get; }
        public string ClassDetailUid { set; get; }
        public int TotalWorkingDaysTillDate { set; get; }
        public int TotalAbsentTillDate { set; get; }
        public IDictionary<string, Double> MonthlyPercentage { set; get; }
        public IList<string> AbsentOn { set; get; }
    }
}

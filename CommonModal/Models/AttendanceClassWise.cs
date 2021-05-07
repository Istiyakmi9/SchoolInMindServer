using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class AttendanceClassWise
    {
        public string AttendanceUid { set; get; }
        public string StudentUid { set; get; }
        public string TanentUid { set; get; }
        public string ClassDetailUid { set; get; }
        public int AttendanceMonth { set; get; }
        public int AttendanceYear { set; get; }
        public bool Day1 { set; get; }
        public bool Day2 { set; get; }
        public bool Day3 { set; get; }
        public bool Day4 { set; get; }
        public bool Day5 { set; get; }
        public bool Day6 { set; get; }
        public bool Day7 { set; get; }
        public bool Day8 { set; get; }
        public bool Day9 { set; get; }
        public bool Day10 { set; get; }
        public bool Day11 { set; get; }
        public bool Day12 { set; get; }
        public bool Day13 { set; get; }
        public bool Day14 { set; get; }
        public bool Day15 { set; get; }
        public bool Day16 { set; get; }
        public bool Day17 { set; get; }
        public bool Day18 { set; get; }
        public bool Day19 { set; get; }
        public bool Day20 { set; get; }
        public bool Day21 { set; get; }
        public bool Day22 { set; get; }
        public bool Day23 { set; get; }
        public bool Day24 { set; get; }
        public bool Day25 { set; get; }
        public bool Day26 { set; get; }
        public bool Day27 { set; get; }
        public bool Day28 { set; get; }
        public bool Day29 { set; get; }
        public bool Day30 { set; get; }
        public bool Day31 { set; get; }
        public DateTime CreatedOn { set; get; }
        public DateTime UpdatedOn { set; get; }
        public string CreatedBy { set; get; }
        public string UpdateBy { set; get; }
    }

    public class AttendanceSingleData
    {
        public string StudentUid { set; get; }
        public string StudentName { set; get; }
        public string ClassDetailUid { set; get; }
        public DateTime Date { set; get; }
        public bool IsPresent { set; get; }
    }
}

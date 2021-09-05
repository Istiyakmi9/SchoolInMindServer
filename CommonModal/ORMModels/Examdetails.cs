using System;
using System.Collections.Generic;

namespace CommonModal.ORMModels
{
    public class Examdetails
    {
        public string ExamDetailId { set; get; }
        public string TanentUid { set; get; }
        public long RoomUid { set; get; }
        public string ExamDescriptionId { set; get; }
        public string Class { set; get; }
        public string SubjectId { set; get; }
        public DateTime? ExamDate { set; get; }
        public string StartTime { set; get; }
        public string EndTime { set; get; }
        public float? Duration { set; get; }
        public int? AcademicYearFrom { set; get; }
        public int? AcademicYearTo { set; get; }
        public string FacultyUid { set; get; }
        public string AdminUid { set; get; }
        public int AccedemicStartYear { set; get; }
    }
}

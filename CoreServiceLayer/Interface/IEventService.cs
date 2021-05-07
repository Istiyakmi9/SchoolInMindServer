using CommonModal.Models;
using CommonModal.ORMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IEventService<T>
    {
        string TimetableByFilterService(string ClassDetailUid);
        string ApplicationTimeSettingService(TimeSettingModal timeSettingModal);
        string FacultyWidthSubjectService(string SubjectUid);
        string AllSubjectService(SearchModal searchModal);
        string SaveAssignedTimetableService(AssignTeacher ObjAssignTeacher);
        string GetSchoolEvents(string SearchStr);
        string SaveSchoolCalendarChangesService(SchoolCalendar ObjSchoolCalendar);
        DataSet GetApplicationSettingService();
        string GetFacultyWithSubjectService();
        string AllocateSubjectService(TimetableAllocationModal timetableAllocationModal);
    }

    public class AssignTeacher
    {
        public string TimetableUid { set; get; }
        public int Period { set; get; }
        public string FacultyName { set; get; }
        public string FacultyUid { set; get; }
        public string SubjectName { set; get; }
        public string SubjectUid { set; get; }
        public string DayName { set; get; }
        public string SubstitutedFacultyUid { set; get; }
        public string ClassDetailUid { set; get; }
        public string AdminId { set; get; }
    }
}

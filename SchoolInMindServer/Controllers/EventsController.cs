using BottomhalfCore.Annotations;
using CommonModal.Models;
using CommonModal.ORMModels;
using SchoolInMindServer.Controllers;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;
using System;

namespace SchoolInMindServer.Controllers
{
    public class EventsController : BaseController
    {
        // GET: Events
        private readonly IEventService<EventService> eventService;
        public EventsController(EventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpPost]
        [Route("Events/SaveCalendarChanges")]
        public IResponse<ApiResponse> SaveCalendarChanges(SchoolCalendar ObjSchoolCalendar)
        {
            string QueryStatus = null;
            if (ObjSchoolCalendar != null)
            {
                QueryStatus = eventService.SaveSchoolCalendarChangesService(ObjSchoolCalendar);
                if (QueryStatus != "")
                {
                    var Result = QueryStatus.Split(':');
                    if (Result.Length > 0)
                        QueryStatus = Result[0];
                }
            }
            return null;//JsonConvert.SerializeObject(QueryStatus);
        }
        public IResponse<ApiResponse> Calender()
        {
            string SearchStr = string.Format("1=1 and Month(StartDate) = {0} and Year(StartDate) = {1}", DateTime.Now.Month, DateTime.Now.Year);
            string CurrentCalendarResult = eventService.GetSchoolEvents(SearchStr);
            if (string.IsNullOrEmpty(CurrentCalendarResult))
                CurrentCalendarResult = "";
            //ViewBag.CurrentEvent = CurrentCalendarResult;
            return null;
        }

        public IResponse<ApiResponse> Contacts(AuthUser objAuthUser)
        {
            return null;
        }

        public IResponse<ApiResponse> Projects()
        {
            return null;
        }

        public IResponse<ApiResponse> ProjectDetail()
        {
            return null;
        }

        public IResponse<ApiResponse> UserProfile(AuthUser objAuthUser)
        {
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> SaveCalendarDetail()
        {
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> AssignTeachNow(AssignTeacher ObjAssignTeacher)
        {
            string Result = null;
            Result = eventService.SaveAssignedTimetableService(ObjAssignTeacher);
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> GetFacultyWidthSubject(string SubjectUid)
        {
            string ProcessingResult = null;
            ProcessingResult = eventService.FacultyWidthSubjectService(SubjectUid);
            return null;// Json(ProcessingResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public IResponse<ApiResponse> TimetableByFilter(string ClassDetailUid)
        {
            string ProcessingResult = null;
            ProcessingResult = eventService.TimetableByFilterService(ClassDetailUid);
            return null; ;
        }

        [HttpPost]
        [Route("api/Events/ApplicationTimeSetting")]
        public IResponse<ApiResponse> ApplicationTimeSetting([FromBody] TimeSettingModal timeSettingModal)
        {
            string Result = eventService.ApplicationTimeSettingService(timeSettingModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/Events/GetApplicationSetting")]
        public IResponse<ApiResponse> GetApplicationSetting()
        {
            var Result = eventService.GetApplicationSettingService();
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/Events/GetTimetable")]
        public IResponse<ApiResponse> Timetable(string ClassDetailUid)
        {
            string Result = eventService.TimetableByFilterService(ClassDetailUid);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/Events/GetFacultyWithSubject")]
        public IResponse<ApiResponse> GetFacultyWithSubject()
        {
            string Result = eventService.GetFacultyWithSubjectService();
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/Events/AllocateSubject")]
        public IResponse<ApiResponse> AllocateSubject([FromBody] TimetableAllocationModal timetableAllocationModal)
        {
            string Result = eventService.AllocateSubjectService(timetableAllocationModal);
            Result = eventService.TimetableByFilterService(timetableAllocationModal.ClassDetailUid);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/Events/AllSubjects")]
        public IResponse<ApiResponse> AllSubjects([FromBody] SearchModal searchModal)
        {
            string Result = eventService.AllSubjectService(searchModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("api/Events/DeleteSchoolPeriodSetting")]
        public IResponse<ApiResponse> DeleteSchoolPeriodSetting([FromBody] RuleBook ruleBook)
        {
            var Result = eventService.DeleteSchoolPeriodSettingService(ruleBook);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }
    }
}
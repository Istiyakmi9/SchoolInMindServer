using BottomhalfCore.Annotations;
using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;
using System.Collections.Generic;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    [Authorize(Policy = Policies.Admin)]
    public class ReportsController : BaseController
    {
        // GET: Reports
        private readonly IAttendenceService<AttendenceService> attendenceService;
        private readonly IReportService<ReportService> reportService;
        private readonly BeanContext context;
        public ReportsController(AttendenceService attendenceService, ReportService reportService)
        {
            this.attendenceService = attendenceService;
            this.reportService = reportService;
            this.context = BeanContext.GetInstance();
            //this.objReportService.CurrentKey = CurrentRequestKey;
        }

        public IResponse<ApiResponse> Faculty()
        {
            //ViewBag.Result = FacultyReports(null, null, null, null);
            return null;
        }

        public IResponse<ApiResponse> Stuff()
        {
            //ViewBag.Result = StaffReport(null, null, null, null);
            return null;
        }

        public IResponse<ApiResponse> Attendence()
        {
            string ResultSet = null;
            ResultSet = JsonConvert.SerializeObject(new List<string>());
            //ViewBag.AttendenceSet = ResultSet;
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> AttendenceReportByFilter(AttendenceFilter ObjAttendenceFilter)
        {
            var result = attendenceService.AttendenceReportByFilterService(ObjAttendenceFilter);
            return null;
        }

        [HttpGet]
        public IResponse<ApiResponse> AttendenceReport(string FromDate, string ToDate, string ClassDetailUid)
        {
            string result = null;
            result = attendenceService.ClassAttendenceRepost(FromDate, ToDate, ClassDetailUid);
            return null;
        }

        [HttpPost]
        [Route("api/Reports/StudentReports")]
        [Authorize]
        public IResponse<ApiResponse> StudentReports([FromBody] SearchModal searchModal)
        {
            var Result = reportService.StudentReportService(searchModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/Reports/Guardian")]
        [Authorize(Roles = Policies.Admin)]
        public IResponse<ApiResponse> GuardianReports([FromBody] SearchModal searchModal)
        {
            var Result = reportService.GuardianReportService(searchModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/Reports/StaffReport")]
        [Authorize(Roles = Policies.Admin)]
        public IResponse<ApiResponse> StaffReport([FromBody] SearchModal searchModal)
        {
            var Result = reportService.StaffReportService(searchModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/Reports/FacultyReports")]
        [Authorize(Roles = Policies.Admin)]
        public IResponse<ApiResponse> FacultyReports([FromBody] SearchModal searchModal)
        {
            var Result = reportService.FacultyReportService(searchModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        public IResponse<ApiResponse> AttendenceAllByFilter(SearchModal searchModal)
        {
            string ClassSectionFilter = " cd.Class = 1";
            var Result = attendenceService.AttendenceAllByFilterService(searchModal, ClassSectionFilter);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public IResponse<ApiResponse> ParentDetailByMobile(string StudentUid)
        {
            var Result = reportService.ParentDetailByMobileService(StudentUid);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }
    }
}
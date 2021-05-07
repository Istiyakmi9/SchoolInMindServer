using BottomhalfCore.Annotations;
using CommonModal.Models;
using CommonModal.ORMModels;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;
using System.Collections.Generic;
using System.Data;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class AdminMasterController : BaseController
    {
        // GET: AdminMaster

        private readonly IAdminMasterService<AdminMasterService> adminMasterService;
        private readonly ICommonService<CommonService> commonService;
        public AdminMasterController(AdminMasterService adminMasterService, CommonService commonService)
        {
            this.adminMasterService = adminMasterService;
            this.commonService = commonService;
        }
        public ActionResult ViewClasses(SearchModal searchModal)
        {
            var result = GetClassDetail(searchModal);
            //ViewBag.Vehicles = result.Data;
            //ViewBag.Type = context.Stringify("vehicle");
            return null;
        }

        public ActionResult ExamSetting()
        {
            //ViewBag.Data = adminMasterService.GetExamDescription();
            return null;
        }

        [HttpPost]
        [Route("AdminMaster/ExamDataInsertion")]
        public IResponse<ApiResponse> ExamDataInsertion(Examdetails ObjExamdetails)
        {
            string Result = "";
            Result = adminMasterService.ExamDataInsertion(ObjExamdetails);
            var ResultData = ExamDetail(0);
            return null;
        }

        [HttpGet]
        [Route("AdminMaster/ExamDetail")]
        public IResponse<ApiResponse> ExamDetail(int Year)
        {
            if (Year <= 0)
                Year = 0;
            string Data = adminMasterService.GetExamDetail(Year);
            return null;// JsonConvert.SerializeObject(Data);
        }

        [HttpGet]
        public IResponse<ApiResponse> GetSubjectByClassSection(int Class, string Section)
        {
            string result;
            result = commonService.GetSubjectByClassSectionService(Class, Section);
            return null;// JsonConvert.SerializeObject(result);
        }

        public IResponse<ApiResponse> ViewAddSubjects(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            string result;
            if (string.IsNullOrEmpty(SearchStr))
                SearchStr = "1=1";
            if (string.IsNullOrEmpty(SortBy))
                SortBy = "Index";
            if (string.IsNullOrEmpty(PageIndex))
                PageIndex = "1";
            if (string.IsNullOrEmpty(PageSize))
                PageSize = "10";
            result = adminMasterService.ViewAddSubjectService(SearchStr, SortBy, PageIndex, PageSize);
            //ViewBag.Subjects = result;
            //ViewBag.Type = context.Stringify("subject");
            return null;
        }

        [HttpPost]
        [Route("api/AdminMaster/InsertNewClassInfo")]
        public IResponse<ApiResponse> InsertNewClassInfo([FromBody] Classdetail objClassdetail)
        {
            DataSet ds = null;
            string ResultSet = adminMasterService.InsertNewClassInfoService(objClassdetail);
            if (ResultSet != null)
                ds = adminMasterService.GetClassesService(new SearchModal());
            return BuildResponse(ds, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public IResponse<ApiResponse> GetVehicleType()
        {
            string ResultSet = null;
            var result = adminMasterService.GetVehicleTypeService();
            if (result != null)
                ResultSet = result;
            return null;// JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        [Route("api/MasterData/AddEditSubjects")]
        public IResponse<ApiResponse> AddEditSubjects([FromBody] Subject ObjSubject)
        {
            this.adminMasterService.AddEditSubjectService(ObjSubject);
            string ResultSet = adminMasterService.ViewAddSubjectService(null, null, null, null);
            return BuildResponse(ResultSet, System.Net.HttpStatusCode.OK);;
        }

        [HttpPost]
        [Route("MasterData/DeleteSubject")]
        public IResponse<ApiResponse> DeleteSubject(Subject ObjSubject)
        {
            string Result = adminMasterService.AddEditSubjectService(ObjSubject);
            return null;// JsonConvert.SerializeObject(Result);
        }

        [HttpDelete]
        [Route("api/AdminMaster/DeleteClassDetail/{Class}/{Section}")]
        public IResponse<ApiResponse> DeleteClassAndSection(int Class, string Section)
        {
            var result = adminMasterService.DeleteClassAndSectionService(Class, Section);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/AdminMaster/GetClassDetail")]
        public IResponse<ApiResponse> GetClassDetail(SearchModal searchModal)
        {
            var result = adminMasterService.GetClassesService(searchModal);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/AdminMaster/AttendenceByClassDetail")]
        public IResponse<ApiResponse> AttendenceByClassDetail([FromBody] Classdetail classdetail)
        {
            string result = adminMasterService.AttendenceByClassDetailService(classdetail);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/AdminMaster/AddUpdateAttendence")]
        public IResponse<ApiResponse> AddUpdateAttendence([FromBody] List<AttendanceClassWise> attendanceClassWise)
        {
            string result = adminMasterService.AddUpdateAttendenceService(attendanceClassWise);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/AdminMaster/AddUpdateSingleClassAttendence")]
        public IResponse<ApiResponse> AddUpdateSingleClassAttendence([FromBody] List<AttendanceSingleData> attendanceSingleData)
        {
            string result = adminMasterService.AddUpdateSingleClassAttendenceService(attendanceSingleData);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/AdminMaster/AddUpdateRoles")]
        public IResponse<ApiResponse> AddUpdateRoles([FromBody] MenuAndRoles menuAndRoles)
        {
            string result = adminMasterService.AddUpdateRoleService(menuAndRoles);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/AdminMaster/GetRoles")]
        public IResponse<ApiResponse> GetRoles()
        {
            string result = adminMasterService.GetRoleService();
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/AdminMaster/RolesByAccessLevel")]
        public IResponse<ApiResponse> RolesByAccessLevel(string AccessLevelUid)
        {
            string result = adminMasterService.RolesByAccessLevelService(AccessLevelUid);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/AdminMaster/GetAllMenu")]
        public IResponse<ApiResponse> GetAllMenu()
        {
            string result = adminMasterService.GetAllMenuService();
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/AdminMaster/ExamDescription")]
        public IResponse<ApiResponse> ExamDescription()
        {
            string ResultSet = adminMasterService.ExamDescriptionService();
            return BuildResponse(ResultSet, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/AdminMaster/InsertExamDetail")]
        public IResponse<ApiResponse> InsertExamDetail([FromBody] ExamDescription examDescription)
        {
            adminMasterService.InsertResultDescriptionService(examDescription);
            string Result = adminMasterService.ExamDescriptionService();
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/AdminMaster/MoveMenu")]
        public IResponse<ApiResponse> MoveMenuItem([FromBody] MenuAndRolesModal menuAndRolesModal)
        {
            string Result = adminMasterService.MoveMenuItemService(menuAndRolesModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }
    }
}
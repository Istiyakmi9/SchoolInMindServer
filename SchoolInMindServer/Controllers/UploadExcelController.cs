using BottomhalfCore.Annotations;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class UploadExcelController : ControllerBase
    {
        // GET: UploadExcel
        IUploadExcelService<UploadExcelService> objUploadExcelService;
        public UploadExcelController(UploadExcelService objUploadExcelService)
        {
            this.objUploadExcelService = objUploadExcelService;
        }
        public IResponse<ApiResponse> Viewdroppickzone()
        {
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> UploadClassDetail(UploadXml objClassdetail)
        {
            string result = "Inserted";
            result = this.objUploadExcelService.UploadClassDetail(objClassdetail, "sp_ClassDetail_InsertBulkByXml");
            if (result == "" || result == null)
                result = "Fail";
            return null;// Json(result);
        }

        [HttpPost]
        public IResponse<ApiResponse> UploadFacultyDetail(UploadXml objClassdetail)
        {
            string result = "Inserted";
            result = this.objUploadExcelService.UploadFacultyDetail(objClassdetail, "sp_FacultyDetail_InsertBulkByXml");
            if (result == "" || result == null)
                result = "Fail";
            return null;// Json(result);
        }

        [HttpPost]
        public IResponse<ApiResponse> UploadVehicleDriverDetail(UploadXml objClassdetail)
        {
            string result = "Inserted";
            result = this.objUploadExcelService.UploadClassDetail(objClassdetail, "");
            if (result == "" || result == null)
                result = "Fail";
            return null;// Json(result);
        }

        [HttpPost]
        public IResponse<ApiResponse> UploadStudentAttendenceDetail(UploadXml objClassdetail)
        {
            string result = "Inserted";
            result = this.objUploadExcelService.UploadStudentAttendenceDetail(objClassdetail, "sp_StudentAttendence_InsertBulkByXml");
            if (result == "" || result == null)
                result = "Fail";
            return null;// Json(result);
        }

        [HttpPost]
        public IResponse<ApiResponse> UploadStudentDetail(UploadXml objClassdetail)
        {
            string result = "";
            result = this.objUploadExcelService.UploadStudentDetailService(objClassdetail, "sp_StudentDetail_BulkByXml");
            if (result == "" || result == null)
                result = "Fail";
            return null;// Json(result);
        }
    }
}                                                                                                             
using BottomhalfCore.Annotations;
using CommonModal.ORMModels;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class ResultController : ControllerBase
    {
        // GET: Result
        private readonly IExamResultService<ExamResultService> objExamResultService;
        public ResultController(ExamResultService objExamResultService)
        {
            this.objExamResultService = objExamResultService;
        }
        public IResponse<ApiResponse> Viewresult()
        {
            return null;
        }

        public IResponse<ApiResponse> ViewResultData()
        {
            //ViewBag.ViewBagData = this.objExamResultService.ViewResultDataService(null, null, 0, 0);
            return null;// View();
        }

        [HttpPost]
        [Route("Result/AddEditResult")]
        public IResponse<ApiResponse> AddEditResult(ExamResult ObjExamResult)
        {
            // ViewBag.ViewBagData = this.objExamResultService.AddEditResultService(ObjExamResult);
            return null;// Json(ViewBag.ViewBagData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public IResponse<ApiResponse> GetResultByRegId(ExamResultPostData objExamResultPostData)
        {
            string Result = null;
            Result = this.objExamResultService.GetResultByRegistrationNo(objExamResultPostData);
            return null;// Json(Result);
        }

        public IResponse<ApiResponse> Showresultbyreg(string RegistrationNo)
        {
            return null;
        }
    }
}
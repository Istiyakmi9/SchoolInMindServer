using CommonModal.Models;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarksController : BaseController
    {
        private readonly IMarkService markService;
        public MarksController(MarkService markService)
        {
            this.markService = markService;
        }

        [HttpPost]
        [Route("fetchmarks")]
        public IResponse<ApiResponse> FetchMarks(SearchModal searchModal)
        {
            string result = this.markService.FetchMarksService(searchModal);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("managemarks")]
        public IResponse<ApiResponse> ManageMarks(Marks marks)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("DeleteMarks")]
        public IResponse<ApiResponse> DeleteMarks(Marks marks)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }
    }
}

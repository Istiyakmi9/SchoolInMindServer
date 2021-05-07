using CommonModal.Models;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeController : BaseController
    {
        private readonly INoticeService noticeService;
        public NoticeController(NoticeService noticeService)
        {
            this.noticeService = noticeService;
        }

        [HttpPost]
        [Route("fetchnotice")]
        public IResponse<ApiResponse> FetchNotice(SearchModal searchModal)
        {
            string result = this.noticeService.FetchNoticeService(searchModal);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("managenotice")]
        public IResponse<ApiResponse> ManageNotice(Notice feesDetail)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("DeleteNotice")]
        public IResponse<ApiResponse> DeleteNotice(Notice feesDetail)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }
    }
}

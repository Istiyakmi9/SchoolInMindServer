using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonModal.Models;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : BaseController
    {
        private readonly IAnnouncementService announcement;
        public AnnouncementController(AnnouncementService announcement)
        {
            this.announcement = announcement;
        }


        [HttpPost]
        [Route("fetchannouncement")]
        public IResponse<ApiResponse> FetchAnnouncement(FetchAnnouncement searchModal)
        {
            String result = this.announcement.GetAllAnnouncementService(searchModal);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("manageannouncement")]
        public IResponse<ApiResponse> ManageAnnouncement(Announcement feesDetail)
        {

            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("DeleteAnnouncement")]
        public IResponse<ApiResponse> DeleteAnnouncement(Announcement feesDetail)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }
    }
}

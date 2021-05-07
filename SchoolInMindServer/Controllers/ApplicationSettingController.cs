using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BottomhalfCore.Annotations;
using CommonModal.Models;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;
using SchoolInMindServer.Controllers;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationSettingController : BaseController
    {
        private readonly IApplicationSettingService<ApplicationSettingService> applicationSettingService;
        public ApplicationSettingController(ApplicationSettingService applicationSettingService)
        {
            this.applicationSettingService = applicationSettingService;
        }

        [HttpPost]
        [Route("CreateOrUpdateServices")]
        public IResponse<ApiResponse> CreateOrUpdateServices([FromBody] List<StoreZone> storeZone)
        {
            string Result = applicationSettingService.CreateOrUpdateServices(storeZone);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("DeleteService/{storeId}")]
        public IResponse<ApiResponse> DeleteService(int storeId)
        {
            var Result = applicationSettingService.DeleteService(storeId);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("GetZone")]
        public IResponse<ApiResponse> GetZone()
        {
            var Result = applicationSettingService.GetZone();
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("CreateRooms/{RoomsCount}")]
        public IResponse<ApiResponse> CreateRooms(int RoomsCount)
        {
            string Result = applicationSettingService.CreateRoomService(RoomsCount);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("GetRoomDetail")]
        public IResponse<ApiResponse> GetRoomDetail([FromBody] SearchModal searchModal)
        {
            string Result = applicationSettingService.GetRoomService(searchModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("UpdateCreateRoomData")]
        public IResponse<ApiResponse> UpdateCreateRoomData([FromBody] RoomDetail roomDetail)
        {
            string Result = applicationSettingService.UpdateRoomDetailService(roomDetail);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }
    }
}
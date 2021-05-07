using CommonModal.Models;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : BaseController
    {
        private readonly IHolidayService holidayService;
        public HolidaysController(HolidayService holidayService)
        {
            this.holidayService = holidayService;
        }

        [HttpPost]
        [Route("fetchholidays")]
        public IResponse<ApiResponse> FetchHolidays(SearchModal searchModal)
        {
            string result = this.holidayService.GetHolidayListService(searchModal);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("manageholiday")]
        public IResponse<ApiResponse> ManageHoliday(Holidays holidays)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("DeleteHoliday")]
        public IResponse<ApiResponse> DeleteHoliday(Holidays holidays)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }
    }
}

using BottomhalfCore.Annotations;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class TrackingController : ControllerBase
    {
        // GET: Tracking

        public readonly ITrackingService<TrackingService> objTrackingService;

        public TrackingController(TrackingService objTrackingService)
        {
            this.objTrackingService = objTrackingService;
        }
        public IResponse<ApiResponse> Vehicles(string SearchStr, string SortBy, string PageIndex, string PageSize)
        {
            string result;
            if (string.IsNullOrEmpty(SearchStr))
                SearchStr = "1=1";
            if (string.IsNullOrEmpty(SortBy))
                SortBy = "FirstName";
            if (string.IsNullOrEmpty(PageIndex))
                PageIndex = "1";
            if (string.IsNullOrEmpty(PageSize))
                PageSize = "10";
            result = objTrackingService.GetVehicleService(SearchStr, SortBy, PageIndex, PageSize);
            //ViewBag.Vehicles = result;
            return null; ;
        }

        public IResponse<ApiResponse> GMap()
        {
            return null;
        }

        [HttpGet]
        public string GetMapDetail(string DeviceId)
        {
            return null;
        }
    }
}
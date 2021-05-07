using BottomhalfCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;
using System.Net;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Transient]
    public class DashboardController : BaseController
    {
        [HttpPost]
        [Route("api/GetDashboard")]
        public IResponse<ApiResponse> Dashboard()
        {
            return BuildResponse("", HttpStatusCode.OK, null);
        }
    }
}
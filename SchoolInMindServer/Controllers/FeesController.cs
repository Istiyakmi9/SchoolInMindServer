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
    public class FeesController : BaseController
    {
        private readonly IFeesService feesService;
        public FeesController(FeesService feesService)
        {
            this.feesService = feesService;
        }

        [HttpPost]
        [Route("fetchfees")]
        public IResponse<ApiResponse> FetchFees(SearchModal searchModal)
        {
            string result = this.feesService.FetchFeesService(searchModal);
            return BuildResponse(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("StudentFeesDetail")]
        public IResponse<ApiResponse> StudentFeesDetail(FeesDetail feesDetail)
        {

            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("managefees")]
        public IResponse<ApiResponse> ManageFees(FeesDetail feesDetail)
        {
            return BuildResponse(null, System.Net.HttpStatusCode.OK);
        }
    }
}

using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;

namespace SchoolInMindServer.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IResponse<ApiResponse> apiResponse;
        public BaseController()
        {
            apiResponse = new ApiResponse();
            apiResponse.HttpStatusCode = HttpStatusCode.OK;
        }
        public IResponse<ApiResponse> BuildResponse(dynamic Data, HttpStatusCode httpStatusCode = HttpStatusCode.OK, string Resion = null, string Token = null)
        {
            if (httpStatusCode != HttpStatusCode.OK)
                apiResponse.HttpStatusCode = httpStatusCode;

            apiResponse.AuthenticationToken = Token;
            apiResponse.HttpStatusMessage = Resion;
            apiResponse.ResponseBody = Data;
            return apiResponse;
        }
    }
}

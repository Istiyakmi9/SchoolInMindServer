using BottomhalfCore.Annotations;
using BottomhalfCore.FactoryContext;
using BottomhalfCore.Annotations;
using CommonModal.Models;
using SchoolInMindServer.Controllers;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;
using System.Collections.Generic;
using System.Net;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class QuickRegistrationController : BaseController
    {
        // GET: QuickRegistration
        private readonly IRegistrationService<RegistrationService> registrationService;
        private readonly IAuthenticationService<AuthenticationService> authenticationService;
        private readonly BeanContext context;
        private readonly HttpContext httpContext;

        public QuickRegistrationController(RegistrationService registrationService,
                                            IHttpContextAccessor httpContext,
                                            AuthenticationService authenticationService)
        {
            this.registrationService = registrationService;
            this.authenticationService = authenticationService;
            this.httpContext = httpContext.HttpContext;
            this.context = BeanContext.GetInstance();
        }

        public IResponse<ApiResponse> QuickRegister()
        {
            //ViewBag.StudentData = null;
            return null;
        }

        [HttpPost]
        public IResponse<ApiResponse> ValidateMobileNo(Dictionary<string, string> MobileNos, string TenentId)
        {
            bool IsStudent = true;
            List<string> ReturnedMessage = new List<string>();
            foreach (var MobileNo in MobileNos)
            {
                if (MobileNo.Key.ToLower() != "student")
                    IsStudent = false;
                if (authenticationService.ValidateMobileNo(MobileNo.Value, TenentId, IsStudent))
                {
                    if (IsStudent)
                        ReturnedMessage.Add(MobileNo.Key);
                    else
                        ReturnedMessage.Add(MobileNo.Key);
                }
            }
            return null;// context.Stringify(ReturnedMessage);
        }

        [HttpPost]
        [Route("api/Registration/QuickRegistration")]
        public IResponse<ApiResponse> QuickRegisterStudent()
        {
            StringValues SubmitedStringifyData = default(string);
            StringValues FileData = default(string);
            httpContext.Request.Form.TryGetValue("quickRegistrationObject", out SubmitedStringifyData);
            httpContext.Request.Form.TryGetValue("fileDetail", out FileData);
            if (SubmitedStringifyData.Count > 0)
            {
                QuickRegistrationModal quickRegistrationModal = JsonConvert.DeserializeObject<QuickRegistrationModal>(SubmitedStringifyData[0]);
                List<Files> fileDetail = JsonConvert.DeserializeObject<List<Files>>(FileData);
                if (quickRegistrationModal != null)
                {
                    IFormFileCollection files = httpContext.Request.Form.Files;
                    string Result = this.registrationService.QuickStudentRegistrationService(quickRegistrationModal, fileDetail, files);
                    BuildResponse(Result, HttpStatusCode.OK);
                }
            }
            return apiResponse;
        }
    }
}
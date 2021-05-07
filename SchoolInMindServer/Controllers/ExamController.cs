using BottomhalfCore.Annotations;
using CommonModal.Models;
using CommonModal.ORMModels;
using SchoolInMindServer.Controllers;
using SchoolInMindServer.Modal;
using Microsoft.AspNetCore.Mvc;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    public class ExamController : BaseController
    {
        // GET: Events
        private readonly IExamDataService<ExamDataService> examDataService;
        public ExamController(ExamDataService examDataService)
        {
            this.examDataService = examDataService;
        }

        [HttpGet]
        [Route("api/Exam/ExamDetail")]
        public IResponse<ApiResponse> ExamDetail(string Class, string ExamDescriptionUid)
        {
            var Result = examDataService.ExamDetailService(Class, ExamDescriptionUid);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/Exam/ExamDetailUpdate")]
        public IResponse<ApiResponse> ExamDetailUpdate([FromBody] List<Examdetails> examdetails)
        {
            string Result = examDataService.ExamDetailUpdateService(examdetails);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }
    }
}
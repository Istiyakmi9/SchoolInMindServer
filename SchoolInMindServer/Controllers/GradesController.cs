using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonModal.Models;
using CoreServiceLayer.Implementation;
using CoreServiceLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : BaseController
    {
        private readonly IGradeService gradeService;
        public GradesController(GradeService gradeService)
        {
            this.gradeService = gradeService;
        }

        [HttpPost]
        [Route("AddUpdateGrade")]
        public IResponse<ApiResponse> AddUpdateGrade([FromBody] GradeDetail gradeDetail)
        {
            this.gradeService.InsertUpdateGradeService(gradeDetail);
            return GetGrade();
        }

        [HttpGet]
        [Route("FetchGrade")]
        public IResponse<ApiResponse> GetGrade()
        {
            var Result = this.gradeService.GetGradesService(null);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }
    }
}

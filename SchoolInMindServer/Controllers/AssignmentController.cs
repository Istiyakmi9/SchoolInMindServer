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
    public class AssignmentController : BaseController
    {
        private readonly IAssignmentService assignmentService;
        public AssignmentController(AssignmentService assignmentService)
        {
            this.assignmentService = assignmentService;
        }

        [HttpPost]
        [Route("GetAssignment")]
        public IResponse<ApiResponse> GetNotice(SearchModal searchModal)
        {
            string Result = this.assignmentService.GetAssignments(searchModal);
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("CreateNewAssignment")]
        public IResponse<ApiResponse> CreateNewAssignment(Assignment assignment)
        {
            string Result = null;
            return BuildResponse(Result, System.Net.HttpStatusCode.OK);
        }
    }
}

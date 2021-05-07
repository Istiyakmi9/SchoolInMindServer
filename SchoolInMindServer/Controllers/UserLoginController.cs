using BottomhalfCore.Annotations;
using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using CoreServiceLayer.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolInMindServer.Modal;
using ServiceLayer.Interface;
using System.Data;
using System.Net;

namespace SchoolInMindServer.Controllers
{
    [Route("api/[Controller]")]
    [Authorize]
    public class UserLoginController : BaseController
    {
        private readonly IAuthenticationService<AuthenticationService> authenticationService;
        private IHttpContextAccessor httpContext;

        [Autowired]
        private CurrentSession currentSession;

        [Enabled(Order: 1)]
        public UserLoginController(AuthenticationService authenticationService, CurrentSession currentSession, IHttpContextAccessor httpContext)
        {
            this.currentSession = currentSession;
            this.httpContext = httpContext;
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("AuthenticateUser")]
        [AllowAnonymous]
        public IResponse<ApiResponse> AuthenticateUser([FromBody] AuthUser authUser)
        {
            string Token = string.Empty;
            DataSet Result = null;
            HttpStatusCode Status = HttpStatusCode.OK;
            if (authUser != null)
            {
                (Result, Token) = this.authenticationService.GetLoginUserObject(authUser);
                this.currentSession.Authorization = Token;
                this.httpContext.HttpContext.Response.Headers["Authorization"] = Token;
            }
            else
                return BuildResponse("Invalid user.", HttpStatusCode.BadRequest, null, null);

            if (Result == null)
                return BuildResponse("Invalid user.", HttpStatusCode.InternalServerError, null, null);

            return BuildResponse(Result, Status, null, Token);
        }
    }
}
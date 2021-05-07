using BottomhalfCore.Annotations;
using BottomhalfCore.Annotations;
using CommonModal.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CoreServiceLayer.Implementation;
using ServiceLayer.Interface;

namespace SchoolInMindServer.Controllers
{
    [Transient]
    [Route("api/[controller]")]
    [EnableCors("BottomhalfCORS")]
    public class BottomhalfController : ControllerBase
    {
        private IAuthenticationService<AuthenticationService> authenticationService;
        private readonly IBottomHalfService<BottomHalfService> bottomHalfService;
        public BottomhalfController(BottomHalfService bottomHalfService, AuthenticationService authenticationService)
        {
            this.bottomHalfService = bottomHalfService;
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("ValidateUser")]
        public string ValidateUser(AuthUser ObjAuthUser)
        {
            string Token = this.bottomHalfService.GetAccountByMobile(ObjAuthUser.UserId);
            if (Token != null && Token != "undefined")
                return JsonConvert.SerializeObject(Token);
            return JsonConvert.SerializeObject(null);
        }
    }
}
using AuthenticationToken;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SchoolInMindServer.MiddlewareServices
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string TokenName = "Authorization";

        public RequestMiddleware(RequestDelegate next, IConfiguration configuration, IOptions<AuthCondition> options)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, CurrentSession currentSession, IJwtTokenManager jwtTokenManager)
        {
            currentSession.Authorization = "";
            Parallel.ForEach(context.Request.Headers, header =>
            {
                if (header.Value.FirstOrDefault() != null)
                {
                    if (header.Key == TokenName)
                    {
                        currentSession.Authorization = header.Value.FirstOrDefault();
                        currentSession.CurrentUserDetail = jwtTokenManager.ReadJwtToken(currentSession.Authorization);
                    }
                }
            });

            try
            {
                context.Response.Headers["Authorization"] = currentSession.Authorization;
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.CompleteAsync();
            }
        }
    }
}

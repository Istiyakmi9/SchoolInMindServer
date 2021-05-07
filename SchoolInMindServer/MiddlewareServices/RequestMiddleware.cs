using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using CommonModal.ProcedureModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolInMindServer.MiddlewareServices
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;
        private readonly string TokenName = "Authorization";
        private readonly AuthCondition authCondition;

        public RequestMiddleware(RequestDelegate next, IConfiguration configuration, IOptions<AuthCondition> options)
        {
            this.authCondition = options.Value;
            this.configuration = configuration;
            _next = next;
        }

        public async Task Invoke(HttpContext context, CurrentSession currentSession)
        {
            currentSession.Authorization = "";
            Parallel.ForEach(context.Request.Headers, header =>
            {
                if (header.Value.FirstOrDefault() != null)
                {
                    if (header.Key == TokenName)
                        currentSession.Authorization = header.Value.FirstOrDefault();
                }
            });

            try
            {
                if (!string.IsNullOrEmpty(currentSession.Authorization))
                {
                    string token = currentSession.Authorization.Replace("Bearer", "").Trim();
                    if (token == "")
                    {
                        if (this.authCondition.NoAuthCheckItems.Any(x => context.Request.Path.Value.ToLower().IndexOf(x.ToLower()) > 0))
                            await _next(context);
                    }
                    else
                    {
                        var handler = new JwtSecurityTokenHandler();
                        handler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                        }, out SecurityToken validatedToken);

                        var securityToken = handler.ReadToken(token) as JwtSecurityToken;
                        //var jwtExpValue = long.Parse(securityToken.Claims.FirstOrDefault(x => x.Type == "exp").Value);
                        //DateTime expirationTime = DateTimeOffset.FromUnixTimeSeconds(jwtExpValue).DateTime;
                        var remaningTime = (securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
                        if (remaningTime <= 0)
                        {
                            BeanContext beanContext = BeanContext.GetInstance();
                            beanContext.RemoveToken(token);
                            context.Response.Headers["Authorization"] = string.Empty;
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            await context.Response.CompleteAsync();
                        }
                        else
                        {
                            currentSession.FileUploadFolderName = "UploadedFiles";
                            currentSession.CurrentUserDetail = Container.GetInstance().Get(token, "userdetail") as UserDetail;
                            if (currentSession.CurrentUserDetail == null)
                            {
                                BeanContext beanContext = BeanContext.GetInstance();
                                beanContext.RemoveToken(token);
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                await context.Response.CompleteAsync();
                            }
                            else
                            {
                                context.Response.Headers["Authorization"] = currentSession.Authorization;
                                await _next(context);
                            }
                        }
                    }
                }
                else
                {
                    if (this.authCondition.NoAuthCheckItems.Any(x => context.Request.Path.Value.ToLower().IndexOf(x.ToLower()) > 0))
                        await _next(context);
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.CompleteAsync();
            }
        }
    }
}

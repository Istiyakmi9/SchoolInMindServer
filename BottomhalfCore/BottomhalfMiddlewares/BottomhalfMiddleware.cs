using BottomhalfCore.DiService;
using BottomhalfCore.Factory.FactoryContext;
using BottomhalfCore.Factory.IFactoryContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BottomhalfCore.BottomhalfMiddlewares
{
    public class BottomhalfMiddleware
    {
        private readonly DiQueue diQueue;
        private readonly bool EnableApiManage;
        private readonly RequestDelegate _next;

        public string MethodDivider = @"
        <div class='method-divider'>
            <div class='method-sec'>
                [[METHOD-NAME]]
            </div>
            <div class='api-sec'>
                [[LINK-SECTION]]
            </div> 
        </div>";

        public string APITemplate = @"<li class='link' name='link'>
            <div class='link-dv' name='link-dv'>
                <button onclick='ManageCurrentAPI(""{{API-VALUE}}"")' class='btn btn-s [[COLORED-CLASS]]'>[[METHOD-NAME]]</button>
                <a class='link-item' name='link-item'>{{API-VALUE}}</a>
            </div>
            <div name='link-body' class='link-body'>
                <div class='col-12 row'>
                    <div class='col-10'>
                        <b>Paramter</b>
                    </div>
                    <div class='text-right'>
                        <button class='btn btn-lightgray' onclick='EnableRequest()'>Tyr & use it</button>
                    </div>
                </div>
                <div class='col-12 p-0 content-body'>
                    <table class='table'>
                        <thead>
                            <tr>
                                <th>
                                    Name
                                </th>
                                <th>
                                    Description
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <div class='f-cell'>body<small>* required</small></div>
                                </td>
                                <td>
                                    <div class='f-cell d-block'>Use below object</div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class=''>(body) <small>for request</small></div>
                                </td>
                                <td>
                                    <div class='f-cell-b d-block'>
                                        <b>Example Value</b> | Model
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <div class='d-block' name='reqestbody'>
                                        [[REQUEST-PARAMETERS]]
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <div class='mt-2'>
                                        <button class='btn btn-dark mr-1 d-none' name='btn-invoke' onclick='Invoke()'>Send</button>
                                        <button class='btn color-dark' name='btn-cancel' onclick='CancelRequest()'>Cancel</button>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <div class='mt-2'>
                                        <b>Content Type</b>
                                        <div>
                                            <select name='contenttype' class='control-dd'>
                                                <option value='application/json'>application/json</option>
                                                <option value='application/json'>plain/text</option>
                                            </select>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div>
                    <div class='col-12 row'>
                        <div class='col-7'>
                            <b>Responses</b>
                        </div>
                        <div class='text-right'>
                            <span class='mr-2'><b>Response content type</b></span>
                            <select name='contenttype' class='control-dd'>
                                <option value='application/json'>application/json</option>
                                <option value='application/json'>plain/text</option>
                            </select>
                        </div>
                    </div>
                    <div class='col-12 p-0 content-body d-none' name='response-section'>
                        <table class='table'>
                            <thead>
                                <tr>
                                    <th>
                                        Code
                                    </th>
                                    <th>
                                        Description
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <div class='f-cell' name='status-code'></div>
                                    </td>
                                    <td>
                                        <div class='f-cell d-block' name='status-text'></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <div class='f-cell-b d-block'>
                                            <b>API Result</b> | Model
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <div class='d-none d-block' name='response-text'>
                                            <textarea class='user-value'></textarea>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </li>";
        public string[] Colors = new string[] { "btn-green", "btn-yellow", "btn-red", "btn-blue" };
        public BottomhalfMiddleware(RequestDelegate next, IConfiguration configuration, bool Flag = false)
        {
            diQueue = DiQueue.GetInstance();
            _next = next;
            EnableApiManage = Flag;
        }

        private string PostingParameterJson(List<dynamic> parameters)
        {
            StringBuilder paramBuilder = new StringBuilder();
            int Index = 0;
            while (Index < parameters.Count)
            {
                paramBuilder.AppendLine(JsonConvert.SerializeObject(parameters[Index], new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                }));
                Index++;
            }
            return $"<textarea class='user-value' name='post-data'>{paramBuilder.ToString()}</textarea>";
        }

        private string QueryStringParameterJson(List<dynamic> parameters)
        {
            StringBuilder paramBuilder = new StringBuilder();
            paramBuilder.Append($@"
                        <div class='d-flex form-group'>
                            <div class='col-4'>
                                <span class='form-label'><b>Parameter Name</b></span>
                            </div>
                            <div class='field-box'>
                                <span class='form-label'><b>Field</b></span>
                            </div>
                            <div class='col-4 pl-4'>
                                <span class='form-label'><b>Parameter Type</b></span>
                            </div>
                        </div>
                        <div class='border-bottom'></div>
                    "
            );
            int Index = 0;
            while (Index < parameters.Count)
            {
                Dictionary<string, string> param = parameters[Index] as Dictionary<string, string>;
                foreach (var Item in param)
                {
                    paramBuilder.Append($@"
                        <div class='d-flex form-group'>
                            <div class='col-4'>
                                <span class='form-label'>{Item.Key}</span>
                            </div>
                            <div class='field-box'>
                                <input type='text' class='form-control' placeholder='{Item.Key}' />
                            </div>
                            <div class='col-4 pl-4'>
                                <span class='form-label'>{Item.Value}</span>
                            </div>
                        </div>"
                    );
                }
                Index++;
            }
            return paramBuilder.ToString();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value == "/apimanager")
            {
                var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var filePath = buildDir + @"\BottomhalfMiddlewares\HeaderTemplate.html";
                string PageData = File.ReadAllText(filePath);
                StringBuilder LinkTemplate = new StringBuilder();
                StringBuilder FinalTemplate = new StringBuilder();
                IAPIManagerd<APIManager> aPIManagerd = new APIManager();
                var APIs = aPIManagerd.GetAPIs();
                string Template = null;
                int Index = 0;
                foreach (var APIByMethod in APIs)
                {
                    LinkTemplate = new StringBuilder();
                    Template = MethodDivider.Replace("[[METHOD-NAME]]", APIByMethod.Key);
                    Index = 0;
                    foreach (var Data in APIByMethod.Value)
                    {
                        if (Data.MethodName.ToLower() == "post")
                        {
                            LinkTemplate.Append(APITemplate.Replace("[[METHOD-NAME]]", Data.MethodName)
                                                            .Replace("[[COLORED-CLASS]]", Colors[Index % 4])
                                                            .Replace("{{API-VALUE}}", Data.URL)
                                                            .Replace("{{API-VALUE}}", Data.URL))
                                                            .Replace("[[REQUEST-PARAMETERS]]", PostingParameterJson(Data.Parameters));
                        }
                        else if (Data.MethodName.ToLower() == "get")
                        {
                            LinkTemplate.Append(APITemplate.Replace("[[METHOD-NAME]]", Data.MethodName)
                                                            .Replace("[[COLORED-CLASS]]", Colors[Index % 4])
                                                            .Replace("{{API-VALUE}}", Data.URL)
                                                            .Replace("{{API-VALUE}}", Data.URL))
                                                            .Replace("[[REQUEST-PARAMETERS]]", $"<div class='get-box'>{QueryStringParameterJson(Data.Parameters)}</div>");
                        }
                        Index++;
                    }
                    FinalTemplate.Append(Template.Replace("[[LINK-SECTION]]", LinkTemplate.ToString()));
                }
                await httpContext.Response.WriteAsync(PageData.Replace("[[LINK-DATA]]", FinalTemplate.ToString())
                    .Replace("[[Data]]", JsonConvert.SerializeObject(APIs, new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                    })));
            }
            else
            {
                diQueue.InitScopeQueue(httpContext.TraceIdentifier);
                await _next(httpContext);
                diQueue.RemoveScoped(httpContext.TraceIdentifier);
            }
        }
    }
}

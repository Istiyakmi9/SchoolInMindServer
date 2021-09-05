using AuthenticationToken;
using AuthenticationToken.Model;
using BottomhalfCore.FactoryContext;
using CommonModal.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MultiTypeDocumentConverter.Service;
using Newtonsoft.Json.Serialization;
using SchoolInMindServer.MiddlewareServices;
using SchoolInMindServer.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SchoolInMindServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                //AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: false);

                this.Configuration = config.Build();

                context = BeanContext
                .GetInstance()
                .Load();

                context.SetApplicationDetail(env.ApplicationName, env.ContentRootPath, env.EnvironmentName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IConfiguration Configuration { get; }
        public string CorsPolicy = "BottomhalfCORS";
        public BeanContext context;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSetting = Configuration.GetSection("Jwt").Get<JwtSetting>();
            var currentSession = Configuration.GetSection("SessionData").Get<CurrentSession>();

            services.AddSingleton<JwtSetting>(jwtSetting);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtSetting.Issuer,
                        ValidAudience = jwtSetting.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key))
                    };
                });

            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            //});

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            context.BuildServices(services, new List<string>
            {
                "SchoolInMindServer.Controllers",
                "CoreServiceLayer.Implementation",
                "CommonModal.Models",
                "CommonModal.ORMModels",
                "CommonModal.ProcedureModel"
            }, true, false, Configuration.GetConnectionString("simdb"));
            services.AddHttpContextAccessor();
            services.AddScoped<CurrentSession>(x => currentSession);
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
            services.AddScoped<IDocumentConverter, DocumentConverter>();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .WithExposedHeaders("Authorization");
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                options.AddPolicy(Policies.User, Policies.UserPolicy());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory())),
                RequestPath = "/Files"
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicy);
            app.UseAuthentication();

            app.UseMiddleware<RequestMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using BottomhalfCore.ContextFactoryManager.Code;
using BottomhalfCore.DatabaseLayer.Common.Code;
using BottomhalfCore.Model;
using BottomhalfCore.Services.Code;
using CommonModal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolInMindServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private readonly APIManagerModal aPIManagerModal;
        private readonly TableAutoMapper tableAutoMapper;
        private readonly IDb db;
        public WeatherForecastController(APIManagerModal aPIManagerModal, IDb db, TableAutoMapper tableAutoMapper, CurrentSession currentSession)
        {
            this.aPIManagerModal = aPIManagerModal;
            this.tableAutoMapper = tableAutoMapper;
            this.db = db;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            List<Fields> fields = new List<Fields>()
            {
                new Fields("FirstName",typeof(string)),
                new Fields("LastName", typeof(int))
            };
            var rng = new Random();
            BuildResponse(null, System.Net.HttpStatusCode.OK);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

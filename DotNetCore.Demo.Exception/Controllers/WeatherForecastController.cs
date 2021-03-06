﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.Demo.Exception.Exceptions;

namespace DotNetCore.Demo.Exception.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [MyExceptionFilter]
    public class WeatherForecastController : ControllerBase
    {
        /*********************异常处理的四种方式******************************/
        /*
         * 1，异常处理页
         * 2，异常处理匿名委托
         * 3，异常过滤器，IExceptionFilter
         * 4，异常处理特性，ExceptionFilterAttribute
         */

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public int TestException1()
        {
            //系统异常
            //throw new System.Exception("这是系统异常！");

            //业务异常
            throw new MyServerException("这是业务异常！", 65);

            return 1;
        }

    }
}

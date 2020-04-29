using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCore.Demo.Http.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.Http.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        /****************HttpClientFactory***********************/
        /*应用场景：发送HTTP请求，类似于.net fx下的HttpClient和HttpWebRequest类
         * 1，三种创建模式：工厂模式；命名客户端模式；类型客户端模式。
         * 2，项目开发当中最佳实践是使用类型客户端模式。
         */


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpclient;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpclient = httpClient;
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

        /// <summary>
        /// 工厂模式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> TestFactoryClient()
        {
            var client = _httpclient.CreateClient();
            return await client.GetStringAsync("http://localhost:5002/api/WeatherForecast/Get2");
        }

        /// <summary>
        /// 命名客户端模式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> TestNamedClient()
        {
            var client = _httpclient.CreateClient("NamedHttpClient");
            return await client.GetStringAsync("/api/WeatherForecast/Get2");
        }

        /// <summary>
        /// 类型客户端模式【最佳实践】
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> TestTypedClient([FromServices]TypedClient typedClient)
        {
            return await typedClient.Get();
        }


    }
}

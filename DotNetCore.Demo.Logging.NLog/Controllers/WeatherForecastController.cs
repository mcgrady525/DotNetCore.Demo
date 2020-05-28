using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.Logging.NLog.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
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
        public int TestNLog1()
        {
            try
            {
                _logger.LogDebug("测试写debug日志！");
                _logger.LogInformation("测试写information日志！");
                _logger.LogWarning("测试写warning日志！");
                _logger.LogError("测试写error日志！");
                _logger.LogCritical("测试写critical日志！");
                //throw new Exception("测试异常！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TestNLog1方法发生异常了！");
            }

            return 1;
        }
    }
}

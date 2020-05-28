using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.Logging.Controllers
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
        public int TestLogging1()
        {
            /****************记录日志的最佳姿势*****************************/
            /*
             * 使用依赖注入的方式获取ILogger对象，然后记录日志
             */

            _logger.LogTrace("this is trace log!");
            _logger.LogDebug("this is debug log!");
            _logger.LogInformation("this is information log!");
            _logger.LogWarning("this is warning log!");
            _logger.LogError("this is error log!");
            _logger.LogCritical("this is critical log!");

            return 1;
        }

        //[HttpGet]
        //public int TestLogging2()
        //{
        //    /****************日志作用域*******************/
        //    /*
        //     * 1，一个事务包含多条操作时
        //     * 2，复杂流程的日志关联时
        //     * 3，调用链追踪与请求处理过程对应时
        //     */
        //    _logger.LogInformation("开始了！");

        //    //模拟业务处理
        //    System.Threading.Thread.Sleep(500);


        //    _logger.LogInformation("结束了！");


        //    return 1;
        //}

        //[HttpGet]
        //public int TestLogging3()
        //{
        //    /****************结构化日志组件Serilog*******************/
        //    /*
        //     * 1，实现日志告警
        //     * 2，实现上下文的关联
        //     * 3，实现与追踪系统集成
        //     */

        //    _logger.LogInformation("测试Serilog开始！");

        //    //模拟业务处理
        //    System.Threading.Thread.Sleep(500);

        //    _logger.LogInformation("测试Serilog结束！");

        //    return 1;
        //}


    }
}

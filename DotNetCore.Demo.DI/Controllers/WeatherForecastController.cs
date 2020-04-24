using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Demo.DI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.DI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        /********演示了两种依赖注入的实例的获取方式************/
        /*
         * 1，构造函数，适用于多数方法使用的场景。
         * 2，[FromServices]，适用于仅当前方法使用的场景。
         */

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOrderService _orderService;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOrderService orderService, IGenericService<IOrderService> genericService)
        {
            _orderService = orderService;
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
        public int TestDI1([FromServices]IMySingletonService singletonService1,
                                [FromServices]IMySingletonService singletonService2,
                                [FromServices]IMyScopedService scopedService1,
                                [FromServices]IMyScopedService scopedService2,
                                [FromServices]IMyTransientService transientService1,
                                [FromServices]IMyTransientService transientService2)
        {
            Console.WriteLine(string.Format("singletonService1的hashcode:{0}", singletonService1.GetHashCode()));
            Console.WriteLine(string.Format("singletonService2的hashcode:{0}", singletonService2.GetHashCode()));

            Console.WriteLine(string.Format("scopedService1的hashcode:{0}", scopedService1.GetHashCode()));
            Console.WriteLine(string.Format("scopedService2的hashcode:{0}", scopedService2.GetHashCode()));

            Console.WriteLine(string.Format("transientService1的hashcode:{0}", transientService1.GetHashCode()));
            Console.WriteLine(string.Format("transientService2的hashcode:{0}", transientService2.GetHashCode()));
            Console.WriteLine("*******************end****************************");

            return 1;
        }

        [HttpGet]
        public int TestDI2([FromServices]IEnumerable<IOrderService> services)
        {
            foreach (var item in services)
            {
                Console.WriteLine("获取到的实例的hashcode：{0}", item.GetHashCode());
            }

            return 1;
        }

    }
}

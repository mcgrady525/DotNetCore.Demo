using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.FileProvider.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        /*****************文件提供程序***************************/
        /*
         * IFileProvider
         * IFileInfo
         * IDirectoryContents
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
        public int TestFileProvider1()
        {
            IFileProvider provider1 = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);
            //var contents = provider1.GetDirectoryContents("/");
            //foreach (var item in contents)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //嵌入的资源
            IFileProvider provider2 = new EmbeddedFileProvider(typeof(Program).Assembly);
            //var html = provider2.GetFileInfo("emb.html");

            //组合
            IFileProvider provider = new CompositeFileProvider(provider1, provider2);
            var contents = provider.GetDirectoryContents("/");
            foreach (var item in contents)
            {
                Console.WriteLine(item.Name);
            }

            return 1;
        }
    }
}

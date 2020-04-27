using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Demo.Configure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace DotNetCore.Demo.Configure.Controllers
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

        /// <summary>
        /// 内存配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration1()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            //添加内存配置数据源
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"key1","value1" },
                {"key2", "value2" },
                {"section1:key4", "value4" }
            });

            IConfigurationRoot configRoot = builder.Build();
            Console.WriteLine(string.Format("key1:{0}", configRoot["key1"]));
            Console.WriteLine(string.Format("key2:{0}", configRoot["key2"]));

            IConfigurationSection section1 = configRoot.GetSection("section1");
            Console.WriteLine(string.Format("section1_key4:{0}", section1["key4"]));

            return 1;
        }

        /// <summary>
        /// 命令行配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration2(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);

            IConfigurationRoot configRoot = builder.Build();
            Console.WriteLine(string.Format("key1:{0}",configRoot["key1"]));

            return 1;
        }

        /// <summary>
        /// 环境变量配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration3()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            //builder.AddEnvironmentVariables();

            //IConfigurationRoot configurationRoot = builder.Build();
            //Console.WriteLine(string.Format("key1:{0}", configurationRoot["key1"]));

            ////分层键
            //IConfigurationSection section1 = configurationRoot.GetSection("section1");
            //Console.WriteLine(string.Format("key3:{0}", section1["key3"]));
            //IConfigurationSection section2 = configurationRoot.GetSection("section1:section2");
            //Console.WriteLine(string.Format("key4:{0}", section2["key4"]));

            //前缀过滤
            builder.AddEnvironmentVariables("test_");
            IConfigurationRoot configRoot = builder.Build();
            Console.WriteLine(string.Format("test_key5:{0}", configRoot["key5"]));
            Console.WriteLine(string.Format("key2:{0}", configRoot["key2"]));

            return 1;
        }

        /// <summary>
        /// 文件配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration4()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional:false, reloadOnChange:true);//reloadOnChange:true当配置更新了程序自动更新

            IConfigurationRoot configRoot = builder.Build();

            Console.WriteLine(string.Format("key1:{0}", configRoot["key1"]));
            Console.WriteLine(string.Format("key2:{0}", configRoot["key2"]));
            Console.WriteLine(string.Format("key5:{0}", configRoot["key5"]));//不存在
            Console.ReadKey();

            Console.WriteLine(string.Format("key1:{0}", configRoot["key1"]));
            Console.WriteLine(string.Format("key2:{0}", configRoot["key2"]));
            Console.WriteLine(string.Format("key5:{0}", configRoot["key5"]));//不存在
            Console.ReadKey();

            return 1;
        }

        /// <summary>
        /// 文件配置-监视变更
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration5()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);//reloadOnChange:true当配置更新了程序自动更新

            IConfigurationRoot configRoot = builder.Build();
            ChangeToken.OnChange(() => configRoot.GetReloadToken(), () =>
            {
                Console.WriteLine(string.Format("key1:{0}", configRoot["key1"]));
                Console.WriteLine(string.Format("key2:{0}", configRoot["key2"]));
                Console.WriteLine(string.Format("key5:{0}", configRoot["key5"]));//不存在
            });
            Console.ReadKey();

            return 1;
        }

        /// <summary>
        /// 文件配置-绑定到强类型（常用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration6()
        {
            //依赖包：Microsoft.Extensions.Configuration.Binder

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);//reloadOnChange:true当配置更新了程序自动更新
            IConfigurationRoot configRoot = builder.Build();

            var scConfig = new StrongClassConfig();
            configRoot.Bind(scConfig, item=> { item.BindNonPublicProperties = true; });

            Console.WriteLine(string.Format("key1:{0}", scConfig.key1));
            Console.WriteLine(string.Format("key3:{0}", scConfig.key3));
            Console.WriteLine(string.Format("key4:{0}", scConfig.key4));

            return 1;
        }

        /// <summary>
        /// 选项框架
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration7([FromServices]IOrderService orderService)
        {
            //选项框架解除了服务和配置之间的依赖关系
            Console.WriteLine(string.Format("OrderService.ShowMaxOrderCount:{0}", orderService.ShowMaxOrderCount()));

            return 1;
        }

        /// <summary>
        /// 选项框架热更新
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration8([FromServices]IOrderService orderService)
        {
            //单例模式下使用：IOptionsMonitor
            //作用域模式下使用：IOptionsSnapshot
            //服务扩展
            Console.WriteLine(string.Format("OrderService.ShowMaxOrderCount:{0}", orderService.ShowMaxOrderCount()));

            return 1;
        }

        /// <summary>
        /// 选项框架验证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestConfiguration9([FromServices]IOrderService orderService)
        {
            //方法1：直接注册验证函数
            //方法2：使用DataAnnotations特性
            //方法3：实现IValidateOptions<TOptions>接口

            Console.WriteLine(string.Format("OrderService.ShowMaxOrderCount:{0}", orderService.ShowMaxOrderCount()));

            return 1;
        }

    }

    public class StrongClassConfig
    { 
        public string key1 { get; set; }

        public bool key3 { get; set; }

        public int key4 { get; private set; }
    }
}

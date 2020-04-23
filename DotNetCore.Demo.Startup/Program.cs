using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /********************启动顺序总结**************************/
            /*
             1，ConfigureWebHostDefaults：配置应用程序启动时必需的组件，比如配置组件，容器组件。
             2，ConfigureHostConfiguration：配置应用程序启动时必要的配置，比如程序启动时需要监听的端口，url等。
             3，ConfigureAppConfiguration：用来嵌入我们自己的配置文件，供应用程序读取。
             4，ConfigureServices，Startup.ConfigureServices：往容器里注入我们的应用的组件，比如service注入。
             5，Startup.Configure：注入我们的中间件。配置HTTP请求的管道。             
             */


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Console.WriteLine("ConfigureWebHostDefaults");
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(service =>
                {
                    Console.WriteLine("ConfigureServices");
                })
                .ConfigureAppConfiguration(config =>
                {
                    Console.WriteLine("ConfigureAppConfiguration");
                })

                .ConfigureHostConfiguration(config =>
                {
                    Console.WriteLine("ConfigureHostConfiguration");
                });

    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;

namespace DotNetCore.Demo.Logging.Default
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //读取配置
                IConfigurationBuilder configBuilder = new ConfigurationBuilder();
                configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var configRoot = configBuilder.Build();

                //将配置对象注册到容器
                IServiceCollection serviceCollection = new ServiceCollection();
                //serviceCollection.AddSingleton<IConfiguration>(p => configRoot);//???

                //注册日志服务
                serviceCollection.AddLogging(builder =>
                {
                    builder.AddConfiguration(configRoot.GetSection("Logging"));
                    builder.AddConsole();
                });
                serviceCollection.AddTransient<OrderService>();

                ////从容器获取日志工厂对象
                IServiceProvider provider = serviceCollection.BuildServiceProvider();
                //ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();

                ////记录日志，通过CreateLogger获取日志记录器对象
                //var logger = loggerFactory.CreateLogger("alogger");
                //logger.LogDebug("this is a debug log!");

                //记录日志，通过构造函数注入获取日志记录器对象
                var orderService = provider.GetService<OrderService>();
                orderService.WriteLog();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}

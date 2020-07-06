using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;

namespace DotNetCore.Demo.Logging.NLog.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                //读配置
                var config = new ConfigurationBuilder()
                   .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .Build();

                //配置DI和注入服务
                var servicesProvider = new ServiceCollection()
                    .AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddConfiguration(config.GetSection("Logging"));//或loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddNLog();
                    })
                    .AddScoped<IRunner, Runner>()
                    .BuildServiceProvider();

                //调用服务
                var runner = servicesProvider.GetRequiredService<IRunner>();
                runner.DoAction("Action1");

                Console.WriteLine("Press ANY key to exit");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                logger.Error(ex, string.Format("调用Main方法发生异常，message:{0}", ex.Message));
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}

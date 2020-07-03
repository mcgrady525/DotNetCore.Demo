using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.IO;

namespace DotNetCore.Demo.Logging.Serilog.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// 初始化配置
        /// </summary>
        public static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    //.AddEnvironmentVariables()
                    .Build();
            }
        }

        static void Main(string[] args)
        {
            //初始化Serilog
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            //.WriteTo.Console(new RenderedCompactJsonFormatter())
            .WriteTo.File(formatter: new CompactJsonFormatter(), "logs\\allfile-.txt", rollingInterval: RollingInterval.Day)//输出到文件
            .CreateLogger();

            try
            {
                //读配置
                //var config = new ConfigurationBuilder()
                //   .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                //   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //   .Build();

                //配置DI和注入服务
                var servicesProvider = new ServiceCollection()
                    .AddLogging(loggingBuilder =>
                    {
                        // configure Logging with NLog
                        loggingBuilder.ClearProviders();
                        loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        loggingBuilder.AddSerilog(logger:Log.Logger, dispose: true);
                    })
                    .AddScoped<IRunner, Runner>()
                    .BuildServiceProvider();

                //调用服务
                var runner = servicesProvider.GetRequiredService<IRunner>();
                runner.DoAction("Action1");

                Log.Debug("开始初始化...");
                Console.WriteLine("Press ANY key to exit");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Format("调用Main方法发生异常，message:{0}", ex.Message));
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

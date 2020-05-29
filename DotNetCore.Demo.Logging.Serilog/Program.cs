using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace DotNetCore.Demo.Logging.Serilog
{
    public class Program
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
                    .AddEnvironmentVariables()
                    .Build();
            }
        }


        public static void Main(string[] args)
        {
            //初始化Serilog
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console(new RenderedCompactJsonFormatter())
            .WriteTo.File(formatter: new CompactJsonFormatter(), "logs\\allfile-.txt", rollingInterval: RollingInterval.Day)//输出到文件
            .CreateLogger();

            try
            {
                Log.Information("开始初始化...");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "初始化时发生异常");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog(dispose: true);
    }
}

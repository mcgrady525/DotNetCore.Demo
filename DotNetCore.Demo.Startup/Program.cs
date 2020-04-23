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
            /********************����˳���ܽ�**************************/
            /*
             1��ConfigureWebHostDefaults������Ӧ�ó�������ʱ��������������������������������
             2��ConfigureHostConfiguration������Ӧ�ó�������ʱ��Ҫ�����ã������������ʱ��Ҫ�����Ķ˿ڣ�url�ȡ�
             3��ConfigureAppConfiguration������Ƕ�������Լ��������ļ�����Ӧ�ó����ȡ��
             4��ConfigureServices��Startup.ConfigureServices����������ע�����ǵ�Ӧ�õ����������serviceע�롣
             5��Startup.Configure��ע�����ǵ��м��������HTTP����Ĺܵ���             
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

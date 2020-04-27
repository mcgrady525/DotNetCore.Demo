using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.Configure.Services
{
    public interface IOrderService
    {
        int ShowMaxOrderCount();
    }

    public class OrderService: IOrderService
    {
        IOptionsMonitor<OrderServiceOptions> _options;

        public OrderService(IOptionsMonitor<OrderServiceOptions> options)
        {
            _options = options;

            //监视变更
            _options.OnChange(o=> 
            {
                Console.WriteLine(string.Format("配置发生了变化:{0}", o.MaxOrderCount));
            });
        }

        public int ShowMaxOrderCount()
        {
            return _options.CurrentValue.MaxOrderCount;
        }
    }

    public class OrderServiceOptions
    { 
        [Range(1,100)]
        public int MaxOrderCount { get; set; }
    }

    /// <summary>
    /// 服务扩展
    /// </summary>
    public static class OrderServiceExtension
    {
        public static void AddOrderService(this IServiceCollection services, IConfiguration configuration)
        {
            //注册配置
            //注册服务
            //services.Configure<OrderServiceOptions>(configuration.GetSection("OrderService"));

            //选项框架配置验证
            services.AddOptions<OrderServiceOptions>().Configure(o =>
            {
                configuration.GetSection("OrderService").Bind(o);
            }).ValidateDataAnnotations();

            //动态配置
            //services.PostConfigure<OrderServiceOptions>(o => 
            //{
            //    o.MaxOrderCount += 10;
            //});

            services.AddSingleton<IOrderService, OrderService>();
        }
    }

}

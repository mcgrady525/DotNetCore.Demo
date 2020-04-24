using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Demo.DI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.DI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注册服务

            #region 传统注册方式
            //1，传统注册方式
            services.AddSingleton<IMySingletonService, MySingletonService>();
            services.AddScoped<IMyScopedService, MyScopedService>();
            services.AddTransient<IMyTransientService, MyTransientService>();

            services.AddSingleton<IOrderService, OrderService>();
            #endregion

            #region 直接注入实例方式
            //2，直接注入实例的方式
            //services.AddSingleton<IMySingletonService>(new MySingletonService());
            //services.AddSingleton<IMySingletonService>(serviceProvider => { return new MySingletonService(); }); 
            #endregion

            #region 尝试注册方式
            //3，尝试注册
            //services.TryAddSingleton<IOrderService, OrderServiceV2>();//如果已经注册过，不管实现是否相同都不再注册。
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderService>());//如果实现相同就不再注册，如果实现不同就注册进去
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderServiceV2>());
            #endregion

            #region 移除和替换
            //services.RemoveAll<IOrderService>();
            //services.Replace(ServiceDescriptor.Singleton<IOrderService, OrderServiceV2>());
            #endregion

            #region 注册泛型模板
            services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));
            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
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

        /// <summary>
        /// 注册autofac容器
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region 默认注册方式
            //builder.RegisterType<AutofacService>().As<IAutofacService>();
            #endregion

            #region 命名注入
            //builder.RegisterType<AutofacServiceV2>().Named<IAutofacService>("service2"); 
            #endregion

            #region 属性注入
            //builder.RegisterType<MyNameService>();
            //builder.RegisterType<AutofacServiceV2>().As<IAutofacService>().PropertiesAutowired(); 
            #endregion

            #region AOP
            //builder.RegisterType<MyInterceptor>();
            //builder.RegisterType<AutofacServiceV2>().As<IAutofacService>().PropertiesAutowired().InterceptedBy(typeof(MyInterceptor)).EnableInterfaceInterceptors();//接口拦截器
            #endregion

            #region 子容器
            builder.RegisterType<MyNameService>().InstancePerMatchingLifetimeScope("myscope");//意味着其它窗口获取不到
            #endregion

        }

        public ILifetimeScope AutofacContainer { get; private set; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //演示Autofac的注入方式
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //命名注入
            //this.AutofacContainer.ResolveNamed<IAutofacService>("service2").ShowCode();

            //属性注入
            //this.AutofacContainer.Resolve<IAutofacService>().ShowCode();

            //AOP
            //this.AutofacContainer.Resolve<IAutofacService>().ShowCode();

            //子容器
            using (var myscope= this.AutofacContainer.BeginLifetimeScope("myscope"))
            {
                var service0 = myscope.Resolve<MyNameService>();
                using (var scope= myscope.BeginLifetimeScope())
                {
                    var service1 = scope.Resolve<MyNameService>();
                    var service2 = scope.Resolve<MyNameService>();

                    Console.WriteLine(string.Format("service1=service2:", service1== service2));
                    Console.WriteLine(string.Format("service1=service0:", service1 == service0));
                }
            }

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

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
            //ע�����

            #region ��ͳע�᷽ʽ
            //1����ͳע�᷽ʽ
            services.AddSingleton<IMySingletonService, MySingletonService>();
            services.AddScoped<IMyScopedService, MyScopedService>();
            services.AddTransient<IMyTransientService, MyTransientService>();

            services.AddSingleton<IOrderService, OrderService>();
            #endregion

            #region ֱ��ע��ʵ����ʽ
            //2��ֱ��ע��ʵ���ķ�ʽ
            //services.AddSingleton<IMySingletonService>(new MySingletonService());
            //services.AddSingleton<IMySingletonService>(serviceProvider => { return new MySingletonService(); }); 
            #endregion

            #region ����ע�᷽ʽ
            //3������ע��
            //services.TryAddSingleton<IOrderService, OrderServiceV2>();//����Ѿ�ע���������ʵ���Ƿ���ͬ������ע�ᡣ
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderService>());//���ʵ����ͬ�Ͳ���ע�ᣬ���ʵ�ֲ�ͬ��ע���ȥ
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderServiceV2>());
            #endregion

            #region �Ƴ����滻
            //services.RemoveAll<IOrderService>();
            //services.Replace(ServiceDescriptor.Singleton<IOrderService, OrderServiceV2>());
            #endregion

            #region ע�᷺��ģ��
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

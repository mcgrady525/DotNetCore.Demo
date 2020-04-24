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

        /// <summary>
        /// ע��autofac����
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region Ĭ��ע�᷽ʽ
            //builder.RegisterType<AutofacService>().As<IAutofacService>();
            #endregion

            #region ����ע��
            //builder.RegisterType<AutofacServiceV2>().Named<IAutofacService>("service2"); 
            #endregion

            #region ����ע��
            //builder.RegisterType<MyNameService>();
            //builder.RegisterType<AutofacServiceV2>().As<IAutofacService>().PropertiesAutowired(); 
            #endregion

            #region AOP
            //builder.RegisterType<MyInterceptor>();
            //builder.RegisterType<AutofacServiceV2>().As<IAutofacService>().PropertiesAutowired().InterceptedBy(typeof(MyInterceptor)).EnableInterfaceInterceptors();//�ӿ�������
            #endregion

            #region ������
            builder.RegisterType<MyNameService>().InstancePerMatchingLifetimeScope("myscope");//��ζ���������ڻ�ȡ����
            #endregion

        }

        public ILifetimeScope AutofacContainer { get; private set; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //��ʾAutofac��ע�뷽ʽ
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //����ע��
            //this.AutofacContainer.ResolveNamed<IAutofacService>("service2").ShowCode();

            //����ע��
            //this.AutofacContainer.Resolve<IAutofacService>().ShowCode();

            //AOP
            //this.AutofacContainer.Resolve<IAutofacService>().ShowCode();

            //������
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

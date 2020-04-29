using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Demo.Http.Clients;
using DotNetCore.Demo.Http.Controllers;
using DotNetCore.Demo.Http.DelegatingHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Demo.Http
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
            services.AddControllers().AddControllersAsServices();

            //��ʽ1������ģʽ
            services.AddHttpClient();

            //����2�������ͻ���ģʽ
            services.AddSingleton<RequestIdDelegatingHandler>();
            services.AddHttpClient("NamedHttpClient", client =>
            {
                client.DefaultRequestHeaders.Add("client-name", "namedclient");
                client.BaseAddress = new Uri("http://localhost:5002");
            }).SetHandlerLifetime(TimeSpan.FromMinutes(20))
            .AddHttpMessageHandler(provider => provider.GetService<RequestIdDelegatingHandler>());

            //����3�����Ϳͻ���ģʽ�����ʵ����
            services.AddHttpClient<TypedClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5002");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

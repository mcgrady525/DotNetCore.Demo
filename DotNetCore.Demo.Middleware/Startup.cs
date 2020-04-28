using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DotNetCore.Demo.Middleware.Middlewares;

namespace DotNetCore.Demo.Middleware
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
            //注入服务和配置

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //注入中间件
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello middleware!");
            //    await next();
            //    if (!(context.Response.HasStarted))
            //    {
            //        await context.Response.WriteAsync("Hello middleware 222!");
            //    }
            //});

            //app.Map("/abc", abcBuilder =>
            //{
            //    abcBuilder.Use(async (context, next) =>
            //    {
            //        await context.Response.WriteAsync("Hello abcBuilder!");
            //        //await next();
            //        //await context.Response.WriteAsync("Hello2");
            //    });
            //});

            //app.MapWhen(context =>
            //{
            //    return context.Request.Query.Keys.Contains("abc");
            //}, builder =>
            //{
            //    builder.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Hello mapwhen!");
            //    });

            //});

            //注入自定义中间件
            app.UseMyMiddleware();

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

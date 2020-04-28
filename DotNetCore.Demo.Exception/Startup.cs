using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Demo.Exception.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotNetCore.Demo.Exception
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
            services.AddMvc(mvcOptions=> 
            {
                //异常处理方法3：异常过滤器，IExceptionFilter
                //mvcOptions.Filters.Add<MyExceptionFilter>();

                //异常处理方法4：异常处理特性，ExceptionFilterAttribute
                mvcOptions.Filters.Add<MyExceptionFilterAttribute>();
            }).AddJsonOptions(jsonoptions =>
            {
                jsonoptions.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //注入中间件

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //异常处理方法1：异常处理页
            //app.UseExceptionHandler("/Error");

            //异常处理方法2：异常处理匿名委托
            //app.UseExceptionHandler(errApp =>
            //{
            //    errApp.Run(async context =>
            //    {
            //        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            //        IKnownException knownException = exceptionHandlerPathFeature.Error as IKnownException;
            //        if (knownException == null)
            //        {
            //            var logger = context.RequestServices.GetService<ILogger<MyExceptionFilterAttribute>>();
            //            logger.LogError(exceptionHandlerPathFeature.Error, exceptionHandlerPathFeature.Error.Message);
            //            knownException = KnownException.Unknown;
            //            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //        }
            //        else
            //        {
            //            knownException = KnownException.FromKnownException(knownException);
            //            context.Response.StatusCode = StatusCodes.Status200OK;
            //        }
            //        var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();
            //        context.Response.ContentType = "application/json; charset=utf-8";
            //        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(knownException, jsonOptions.Value.JsonSerializerOptions));
            //    });
            //});


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

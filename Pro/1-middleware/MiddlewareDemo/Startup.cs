using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiddlewareDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);
            var logger = loggerFactory.CreateLogger("Middleware Demo");
            if (env.IsStaging())
            {
                Console.WriteLine("Siamo in staging");
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<EnvironmentMiddleware>();
            
            app.Use(async (context, next) =>
            {
                var timer = Stopwatch.StartNew();
                logger.LogInformation($"=================> Start request in: {env.EnvironmentName}");
                await next();
                logger.LogInformation($"=================> Completed request in: {timer.ElapsedMilliseconds}ms");
            });

            app.UseStaticFiles();
            app.Map("/Contatti", a => a.Run(async context =>
            {
                await context.Response.WriteAsync("Ecco i contatti");
            }));
            app.MapWhen(context => context.Request.Headers["User-Agent"].First().Contains("Firefox"), FirefoxRoute);
            app.Run(async (context) =>
            {
                //context.Response.Headers.Add("")
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private void FirefoxRoute(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello Firefox");
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MiddlewareDemo
{
    public class EnvironmentMiddleware
    {
        private IHostingEnvironment _Env;
        private RequestDelegate _Next;
        public EnvironmentMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _Env = env;
            _Next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var timer = Stopwatch.StartNew();
            context.Response.Headers.Add("X-HostingEnvironmentName", new[] { _Env.EnvironmentName });

            await _Next(context);

            if (_Env.IsDevelopment() &&
                context.Response.ContentType != null &&
                context.Response.ContentType == "text/html")
            {
                await context.Response.WriteAsync($"<p>From {_Env.EnvironmentName} in {timer.ElapsedMilliseconds}ms</p>");
            }
        }
    }

    public static class MiddlewareHelpers
    {
        public static IApplicationBuilder UseEnvironmentMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<EnvironmentMiddleware>();
        }
    }
}

# Realizzazione middleware

Per la realizzazione di un middleware all'interno della pipeline di configurazione in `Startup.cs` nel metodo `Configure` occorre realizzare una classe che implementi i seguenti metodi: 

- Il metodo costruttore, che usa gli argomenti passati per inizializzarli;
- il metodo `Invoke` nel quale si passa come argomento il contesto di interesse;

A questo punto il metodo potrebbe essere richiamato in `Startup.cs` utilizzando il metodo `app.UseMiddleware<EnvironmentMiddleware>()`, ma se vogliamo un ulteriore personalizzazione ed utilizzare un metodo più descrittivo, basta utilizzare la seguente classe `MiddlewareHelpers`.
Il metodo pubblico statico all'interno utilizzerà l'interfaccia `IApplicationBuilder` e il nome del metodo sarà quello utilizzato per il middleware.

```cs
    //MiddlewareDemo.cs
        public static class MiddlewareHelpers
        {
            public static IApplicationBuilder UseEnvironmentMiddleware(this IApplicationBuilder app)
            {
                return app.UseMiddleware<EnvironmentMiddleware>();
            }
        }
```

```cs
    // Startup.cs
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
```

```cs
    //MiddlewareDemo.cs
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

```
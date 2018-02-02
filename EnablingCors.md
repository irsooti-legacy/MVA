# CORS 

## What is Cross-Origin Resource Sharing (CORS)
Browser security prevents a web page from making AJAX requests to another domain. This restriction is called the same-origin policy and prevents a malicious site from reading sensitive data from another site.

However, sometimes you might want to let other sites make cross-origin requests to your web app. Cross Origin Resource Sharing (CORS) is a W3C standard that allows a server to relax the same-origin policy. Using CORS, a server can explicitly allow some cross-origin requests while rejecting others.

What is "same origin"?
Two URLs have the same origin if they have identical schemes, hosts, and ports.

These two URLs have the same origin:

- http://example.com/api/foo
- http://example.com/api/bar

These URLs have different origins than the previous two:

- http://example.net - Different domain
- http://example.com:9000 - Different port
- https://example.com - Different scheme
- http://www.example.com - Different subdomain

That means, if a web page of the second group URLs launches an AJAX call to the web API of first group URLs, the AJAX call will be dropped by the web API. To allow "friendly" websites to access the web API cross-origin, we should enable CORS.

## Enable Cross-Origin Resource Sharing (CORS)
It's very simple to enable CORS for an ASP.NET Core Web API application. Only two small changes in the Startup.cs are needed,

- Find ConfigureServices method, add services.AddCors();
- Find Configure method, add the code below,

```cs
// Startup.cs
app.UseCors(builder => builder
    .WithOrigins("http://friend-origin-name.com")
    .WithMethods("GET", "POST", "PUT", "DELETE")
    .AllowAnyHeader());
```

Or, if your web API allows access from any origin, you could add the following code:
```cs
// Startup.cs
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

```

## Further reading

- https://courses.edx.org/courses/course-v1:Microsoft+DEV247x+1T2018
using Microsoft.AspNetCore.Http;

namespace Microservices.Common.Middlewares
{
    public class CustomCorsMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomCorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.Request.Method == HttpMethods.Options)
                {
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization,Content-Type,*");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,PATCH,OPTIONS,HEAD,*");
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    //context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                    await context.Response.WriteAsync("OK");
                }
                else
                {
                    //To add Headers AFTER everything you need to do this
                    context.Response.OnStarting(state =>
                    {
                        var httpContext = (HttpContext)state;
                        //remove CORS headers incase they are returned from elsewhere
                        httpContext.Response.Headers.Remove("Access-Control-Allow-Headers");
                        httpContext.Response.Headers.Remove("Access-Control-Allow-Methods");
                        httpContext.Response.Headers.Remove("Access-Control-Allow-Origin");

                        //readd them again
                        httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization,Content-Type,*");
                        httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,PATCH,OPTIONS,HEAD,*");
                        httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                        //httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                        return Task.CompletedTask;
                    }, context);

                    await _next(context);

                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
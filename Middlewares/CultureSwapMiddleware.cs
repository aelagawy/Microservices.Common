using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace Microservices.Common.Middlewares
{
    public class CultureSwapHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CultureSwapHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var cultureInfo = context.Request.Headers["Accept-Language"].ToString().ToLower().Contains("en") ?
                 new CultureInfo("en-US")
                 : new CultureInfo("ar-EG");


            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
    public static class CultureSwapHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCultureSwapHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureSwapHandlerMiddleware>();
        }
    }
}

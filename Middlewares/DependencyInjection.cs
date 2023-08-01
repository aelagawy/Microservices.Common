using Microsoft.AspNetCore.Builder;

namespace Microservices.Common.Middlewares
{
    public static class DependencyInjection
    {
        public static IApplicationBuilder UseCustomMiddleWares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CustomCorsMiddleware>();
            builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
            builder.UseMiddleware<CultureSwapHandlerMiddleware>();

            return builder;
        }
    }
}

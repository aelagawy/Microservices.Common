using Microservices.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Microservices.Common.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next/*, ILogger logger*/)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //TODO: implement serilog
                //_logger.LogError(ex, "CustomExceptionHandlerMiddleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Failures);
                    break;

                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;

                case UnAuthenticatedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    break;

                case UnAuthorizedAccessException _:
                    code = HttpStatusCode.Forbidden;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}

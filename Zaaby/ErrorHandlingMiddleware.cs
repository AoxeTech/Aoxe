using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Zaaby.Abstractions;

namespace Zaaby
{
    internal class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ZaabyException ex)
            {
                await HandleExceptionAsync(context, ex, 600);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, 600);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, int httpStatusCode)
        {
            var inmostEx = ex;
            while (inmostEx.InnerException != null)
                inmostEx = inmostEx.InnerException;
            context.Response.StatusCode = httpStatusCode;
            ZaabyError zaabyError;
            if (inmostEx is ZaabyException zaabyException)
                zaabyError = new ZaabyError
                {
                    Id = zaabyException.Id,
                    Message = inmostEx.Message,
                    Source = inmostEx.Source,
                    StackTrace = inmostEx.StackTrace
                };
            else
                zaabyError = new ZaabyError
                {
                    Id = Guid.NewGuid(),
                    Message = inmostEx.Message,
                    Source = inmostEx.Source,
                    StackTrace = inmostEx.StackTrace
                };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(zaabyError));
        }
    }

    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder) =>
            builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
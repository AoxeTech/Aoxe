using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Zaaby.Core;

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
                if (context.Response.StatusCode >= 400)
                {
                    var statusCode = context.Response.StatusCode;
                    var msg = ((HttpStatusCode)context.Response.StatusCode).ToString();
                    context.Response.StatusCode = 200;
                    await HandleExceptionAsync(context, new ZaabyException($"{context.Request.Path.Value} httpStatus:{statusCode}"));
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var innerEx = ex;
            while (innerEx.InnerException != null)
                innerEx = innerEx.InnerException;

            var data = new ZaabyDtoBase<ZaabyException>
            {
                Id = Guid.NewGuid(),
                Timespan = DateTimeOffset.UtcNow,
                Msg = innerEx.Message,
                Status = Status.Failure,
                ErrCode = context.Response.StatusCode
            };

            if (innerEx is ZaabyException zaabyException)
                data.Data = zaabyException;
            else
                data.Data = new ZaabyException(innerEx.Message, innerEx);

            var result = JsonConvert.SerializeObject(data);
            return context.Response.WriteAsync(result);
        }
    }

    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
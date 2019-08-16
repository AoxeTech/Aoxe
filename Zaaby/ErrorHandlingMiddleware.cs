using System;
using System.Net;
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
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode httpStatusCode)
        {
            var innerEx = ex;
            while (innerEx.InnerException != null)
                innerEx = innerEx.InnerException;
            context.Response.StatusCode = (int) httpStatusCode;

            if (!context.Request.Headers.ContainsKey("Accept"))
                return context.Response.WriteAsync(JsonConvert.SerializeObject(innerEx));
            switch (context.Request.Headers["Accept"])
            {
                case "application/json": return context.Response.WriteAsync(JsonConvert.SerializeObject(innerEx));
                case "application/x-protobuf": return context.Response.WriteAsync(JsonConvert.SerializeObject(innerEx));
                default: return context.Response.WriteAsync(JsonConvert.SerializeObject(innerEx));
            }
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
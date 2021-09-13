using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Zaaby.Shared;

namespace Zaaby.Server
{
    internal class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

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
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(context, ex, 600);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await HandleExceptionAsync(context, ex, 600);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex, 600);
            }
            catch (ApplicationException ex)
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
            while (inmostEx.InnerException is not null) inmostEx = inmostEx.InnerException;
            context.Response.StatusCode = httpStatusCode;
            var zaabyError = new ZaabyError
            {
                Id = inmostEx is ZaabyException zaabyException ? zaabyException.Id : Guid.NewGuid(),
                Message = inmostEx.Message,
                Source = inmostEx.Source,
                StackTrace = inmostEx.StackTrace,
                ThrowTime = DateTimeOffset.Now
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(zaabyError));
        }
    }
}
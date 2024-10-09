namespace Aoxe.WebApi;

internal class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AoxeException ex)
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
        while (inmostEx.InnerException is not null)
            inmostEx = inmostEx.InnerException;
        context.Response.StatusCode = httpStatusCode;
        var aoxeError = new AoxeError
        {
            Id = inmostEx is AoxeException aoxeException ? aoxeException.Id : Guid.NewGuid(),
            Message = inmostEx.Message,
            Source = inmostEx.Source ?? string.Empty,
            StackTrace = inmostEx.StackTrace ?? string.Empty,
            ThrowTime = DateTime.Now
        };
        if (aoxeError == null)
            throw new ArgumentNullException(nameof(aoxeError));
        return context.Response.WriteAsync(JsonSerializer.Serialize(aoxeError));
    }
}

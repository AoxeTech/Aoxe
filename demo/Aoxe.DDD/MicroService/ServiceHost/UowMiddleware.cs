namespace ServiceHost;

public class UowMiddleware
{
    private readonly RequestDelegate _next;

    public UowMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AoxeDddContext dbContext)
    {
        await _next(context);
        await dbContext.SaveChangesAsync();
    }
}

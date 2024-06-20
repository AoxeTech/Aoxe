namespace Aoxe.Server;

public static class AoxeIApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAoxe(this IApplicationBuilder app) =>
        app.UseAoxeErrorHandling()
            .UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers());

    public static IApplicationBuilder UseAoxeErrorHandling(this IApplicationBuilder app) =>
        app.UseMiddleware<ErrorHandlingMiddleware>();

    public static IApplicationBuilder UseAoxeUnitOfWork(this IApplicationBuilder app) =>
        app.UseMiddleware<UnitOfWorkMiddleware>();
}

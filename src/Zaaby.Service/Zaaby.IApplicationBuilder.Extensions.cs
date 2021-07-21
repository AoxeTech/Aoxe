using Microsoft.AspNetCore.Builder;

namespace Zaaby.Service
{
    public static class ZaabyIApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseZaaby(this IApplicationBuilder app) =>
            app.UseZaabyErrorHandling()
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());

        public static IApplicationBuilder UseZaabyErrorHandling(this IApplicationBuilder app) =>
            app.UseMiddleware<ErrorHandlingMiddleware>();

        public static IApplicationBuilder UseZaabyUnitOfWork(this IApplicationBuilder app) =>
            app.UseMiddleware<UnitOfWorkMiddleware>();
    }
}
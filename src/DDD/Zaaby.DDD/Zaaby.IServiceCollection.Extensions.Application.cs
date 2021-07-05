using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;
using Zaaby.DDD.Abstractions.Application;

namespace Zaaby.DDD
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services) =>
            services.Register<IApplicationService>(ServiceLifetime.Scoped)
                .Register<ApplicationServiceAttribute>(ServiceLifetime.Scoped);

        public static IServiceCollection AddIntegrationEventHandler(this IServiceCollection services) =>
            services.Register<IIntegrationEventHandler>(ServiceLifetime.Scoped)
                .Register<IntegrationEventHandlerAttribute>(ServiceLifetime.Scoped);
    }
}
namespace Aoxe.DDD;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services) =>
        services
            .Register<IApplicationService>(ServiceLifetime.Scoped)
            .Register<ApplicationServiceAttribute>(ServiceLifetime.Scoped);

    public static IServiceCollection AddIntegrationEventHandler(this IServiceCollection services) =>
        services
            .Register<IIntegrationEventHandler>(ServiceLifetime.Scoped)
            .Register<IntegrationEventHandlerAttribute>(ServiceLifetime.Scoped);
}

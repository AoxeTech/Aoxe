namespace Aoxe.DDD;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddDDD(this IServiceCollection services) =>
        services
            .AddApplicationService()
            .AddIntegrationEventHandler()
            .AddDomainService()
            .AddDomainEventHandler()
            .AddDomainFactory()
            .AddRepository()
            .AddDomainEventPublisher();
}

namespace Zaaby.DDD;

public static partial class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection AddDDD(this IServiceCollection services) =>
        services.AddApplicationService()
            .AddIntegrationEventHandler()
            .AddDomainService()
            .AddDomainEventHandler()
            .AddDomainFactory()
            .AddRepository()
            .AddDomainEventPublisher();
}
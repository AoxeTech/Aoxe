namespace Aoxe.DDD;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddDomainService(this IServiceCollection services) =>
        services
            .Register<IDomainService>(ServiceLifetime.Scoped)
            .Register<DomainServiceAttribute>(ServiceLifetime.Scoped);

    public static IServiceCollection AddDomainFactory(this IServiceCollection services) =>
        services
            .Register<IFactory>(ServiceLifetime.Scoped)
            .Register<FactoryAttribute>(ServiceLifetime.Scoped);

    public static IServiceCollection AddDomainEventPublisher(this IServiceCollection services) =>
        services.AddHostedService<DomainEventPublisher>();

    public static IServiceCollection AddDomainEventHandler(this IServiceCollection services) =>
        services
            .Register<IDomainEventHandler>(ServiceLifetime.Scoped)
            .Register<DomainEventHandlerAttribute>(ServiceLifetime.Scoped);
}

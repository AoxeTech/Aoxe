namespace Zaaby.DDD;

public static partial class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services.Register<IRepository>(ServiceLifetime.Scoped)
            .Register<RepositoryAttribute>(ServiceLifetime.Scoped);
}
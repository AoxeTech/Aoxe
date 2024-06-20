namespace Aoxe.DDD;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services
            .Register<IRepository>(ServiceLifetime.Scoped)
            .Register<RepositoryAttribute>(ServiceLifetime.Scoped);
}

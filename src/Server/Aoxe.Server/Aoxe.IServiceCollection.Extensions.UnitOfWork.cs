namespace Aoxe.Server;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork(
        this IServiceCollection services,
        Func<IServiceProvider, IDbTransaction> factory
    )
    {
        services.AddScoped(factory);
        return services;
    }
}

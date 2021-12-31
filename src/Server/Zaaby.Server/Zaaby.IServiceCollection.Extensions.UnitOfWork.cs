namespace Zaaby.Server;

public static partial class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services,
        Func<IServiceProvider, IDbTransaction> factory)
    {
        services.AddScoped(factory);
        return services;
    }
}
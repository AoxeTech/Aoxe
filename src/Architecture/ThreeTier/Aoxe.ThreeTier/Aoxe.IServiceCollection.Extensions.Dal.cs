namespace Aoxe.ThreeTier;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddDal(this IServiceCollection services) =>
        services
            .Register<IDal>(ServiceLifetime.Scoped)
            .Register<DalAttribute>(ServiceLifetime.Scoped);
}

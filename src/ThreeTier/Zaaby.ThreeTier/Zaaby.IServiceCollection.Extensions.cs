namespace Zaaby.ThreeTier;

public static partial class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection AddThreeTier(this IServiceCollection services) =>
        services.AddBll()
            .AddMessageHandler()
            .AddDal();
}
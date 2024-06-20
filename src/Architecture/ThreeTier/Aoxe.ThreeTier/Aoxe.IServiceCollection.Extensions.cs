namespace Aoxe.ThreeTier;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddThreeTier(this IServiceCollection services) =>
        services.AddBll().AddMessageHandler().AddDal();
}

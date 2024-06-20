namespace Aoxe.Shared;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection FromDirectoriesOf(
        this IServiceCollection services,
        params string[] directories
    )
    {
        LoadHelper.FromDirectories(directories);
        return services;
    }
}

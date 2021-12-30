namespace Zaaby.Shared;

public static partial class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection FromDirectoriesOf(this IServiceCollection services,
        params string[] directories)
    {
        LoadHelper.FromDirectories(directories);
        return services;
    }
}
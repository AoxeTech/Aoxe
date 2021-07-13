using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Common
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection FromDirectoriesOf(this IServiceCollection services,
            params string[] directories)
        {
            LoadHelper.FromDirectories(directories);
            return services;
        }
    }
}
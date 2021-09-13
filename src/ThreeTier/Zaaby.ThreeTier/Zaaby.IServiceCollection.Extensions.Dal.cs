using Microsoft.Extensions.DependencyInjection;
using Zaaby.Shared;
using Zaaby.ThreeTier.Annotations.DataAccess;

namespace Zaaby.ThreeTier
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddDal(this IServiceCollection services) =>
            services.Register<IDal>(ServiceLifetime.Scoped)
                .Register<DalAttribute>(ServiceLifetime.Scoped);
    }
}
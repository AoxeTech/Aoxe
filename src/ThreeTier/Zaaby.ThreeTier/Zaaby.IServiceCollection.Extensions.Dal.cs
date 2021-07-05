using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;
using Zaaby.ThreeTier.Abstractions.DataAccess;

namespace Zaaby.ThreeTier
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddDal(this IServiceCollection services) =>
            services.Register<IDal>(ServiceLifetime.Scoped)
                .Register<DalAttribute>(ServiceLifetime.Scoped);
    }
}
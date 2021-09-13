using Microsoft.Extensions.DependencyInjection;
using Zaaby.Shared;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Zaaby.DDD
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services) =>
            services.Register<IRepository>(ServiceLifetime.Scoped)
                .Register<RepositoryAttribute>(ServiceLifetime.Scoped);
    }
}
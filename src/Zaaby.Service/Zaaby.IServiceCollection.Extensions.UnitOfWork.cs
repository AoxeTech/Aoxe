using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Service
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services,
            Func<IServiceProvider, IDbTransaction> factory)
        {
            services.AddScoped(factory);
            return services;
        }
    }
}
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Zaaby
{
    public partial class ZaabyHost
    {
        public ZaabyHost AddHostedService<THostedService>(ZaabyHost zaabyHost)
            where THostedService : class, IHostedService
        {
            TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());
            return zaabyHost;
        }

        public ZaabyHost AddHostedService<THostedService>(ZaabyHost zaabyHost,
            Func<IServiceProvider, THostedService> implementationFactory) where THostedService : class, IHostedService
        {
            TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService>(implementationFactory));
            return zaabyHost;
        }
    }
}
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Zaaby
{
    public partial class ZaabyServer
    {
        public ZaabyServer AddHostedService<THostedService>(ZaabyServer zaabyServer)
            where THostedService : class, IHostedService
        {
            TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());
            return zaabyServer;
        }

        public ZaabyServer AddHostedService<THostedService>(ZaabyServer zaabyServer,
            Func<IServiceProvider, THostedService> implementationFactory)
            where THostedService : class, IHostedService
        {
            TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService>(implementationFactory));
            return zaabyServer;
        }
    }
}
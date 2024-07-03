namespace Aoxe;

public partial class AoxeHost
{
    public AoxeHost AddHostedService<THostedService>(AoxeHost aoxeHost)
        where THostedService : class, IHostedService
    {
        TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());
        return aoxeHost;
    }

    public AoxeHost AddHostedService<THostedService>(
        AoxeHost aoxeHost,
        Func<IServiceProvider, THostedService> implementationFactory
    )
        where THostedService : class, IHostedService
    {
        TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService>(implementationFactory));
        return aoxeHost;
    }
}

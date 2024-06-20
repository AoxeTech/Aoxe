namespace Aoxe;

public partial class AoxeHost
{
    public AoxeHost AddHostedService<THostedService>(AoxeHost AoxeHost)
        where THostedService : class, IHostedService
    {
        TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());
        return AoxeHost;
    }

    public AoxeHost AddHostedService<THostedService>(
        AoxeHost AoxeHost,
        Func<IServiceProvider, THostedService> implementationFactory
    )
        where THostedService : class, IHostedService
    {
        TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService>(implementationFactory));
        return AoxeHost;
    }
}

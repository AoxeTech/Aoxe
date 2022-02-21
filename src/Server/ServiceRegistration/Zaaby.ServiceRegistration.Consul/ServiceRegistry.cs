namespace Zaaby.ServiceRegistration.Consul;

public class ServiceRegistry : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly ZaabeeConsulOptions _options;
    private CancellationTokenSource _cts;

    public ServiceRegistry(IConsulClient consulClient, IOptions<ZaabeeConsulOptions> options)
    {
        _consulClient = consulClient;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        if (_options.AgentServiceRegistration is null) return;
        await _consulClient.Agent.ServiceRegister(_options.AgentServiceRegistration, _cts.Token);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        if (_options.AgentServiceRegistration is not null)
            await _consulClient.Agent.ServiceDeregister(_options.AgentServiceRegistration.ID, cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
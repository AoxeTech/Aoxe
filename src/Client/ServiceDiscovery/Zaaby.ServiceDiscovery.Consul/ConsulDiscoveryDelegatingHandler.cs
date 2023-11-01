namespace Zaaby.ServiceDiscovery.Consul;

public class ConsulServiceDiscoveryDelegatingHandler : DelegatingHandler
{
    private static readonly Random Random = new();
    private readonly IConsulClient _consulClient;

    public ConsulServiceDiscoveryDelegatingHandler(IConsulClient consulClient)
    {
        _consulClient = consulClient;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var current = request.RequestUri;
        try
        {
            var catalogService = await LookupServiceAsync(current.Host);
            var domainName = $"{catalogService.ServiceAddress}:{catalogService.ServicePort}";
            request.RequestUri = new Uri($"{current.Scheme}://{domainName}//{current.PathAndQuery}");
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            request.RequestUri = current;
        }
    }

    private async Task<CatalogService> LookupServiceAsync(string serviceName)
    {
        var services = (await _consulClient.Catalog.Service(serviceName)).Response;
        if (services is null || !services.Any()) return null;
        var index = Random.Next(services.Length);
        return services.ElementAt(index);
    }
}
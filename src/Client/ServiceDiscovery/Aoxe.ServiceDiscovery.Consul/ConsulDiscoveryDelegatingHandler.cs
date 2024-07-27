namespace Aoxe.ServiceDiscovery.Consul;

public class ConsulServiceDiscoveryDelegatingHandler(IConsulClient consulClient) : DelegatingHandler
{
    private static readonly Random Random = new();

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var current = request.RequestUri;
        try
        {
            if (current is null)
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var catalogService = await LookupServiceAsync(current.Host);
            var domainName = $"{catalogService.ServiceAddress}:{catalogService.ServicePort}";
            request.RequestUri = new Uri(
                $"{current.Scheme}://{domainName}//{current.PathAndQuery}"
            );

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            request.RequestUri = current;
        }
    }

    private async Task<CatalogService> LookupServiceAsync(string serviceName)
    {
        var services = (await consulClient.Catalog.Service(serviceName)).Response;
        if (services is null || services.Length is 0)
            return new CatalogService();
        var index = Random.Next(services.Length);
        return services.ElementAt(index);
    }
}

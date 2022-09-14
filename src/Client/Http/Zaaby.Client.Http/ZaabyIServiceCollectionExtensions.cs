using Zaabee.NewtonsoftJson;

namespace Zaaby.Client.Http;

public static class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection AddZaabyClient(this IServiceCollection services,
        Type serviceDefineType,
        Dictionary<string, string> configUrls,
        Action<ZaabyClientFormatterOptions>? optionsFactory = null)
    {
        if (configUrls.Count is 0) return services;

        var typeWithUris = LoadHelper.GetTypePairs(serviceDefineType)
            .Where(p => p.InterfaceType?.Namespace is not null && p.ImplementationType is null)
            .Join(configUrls,
                typePair => typePair.InterfaceType.Namespace,
                configUrl => configUrl.Key,
                (typePair, configUrl) =>
                    new
                    {
                        Type = typePair.InterfaceType,
                        UriString = configUrl.Value
                    });

        var options = new ZaabyClientFormatterOptions(new Serializer(), "application/json");
        optionsFactory?.Invoke(options);
        var formatter = ZaabyHttpClientFormatterFactory.Create(options);

        foreach (var typeWithUri in typeWithUris)
        {
            var @namespace = typeWithUri.Type.Namespace!;
            services.AddHttpClient(@namespace,
                httpClient => httpClient.BaseAddress = new Uri(typeWithUri.UriString));
            services.AddScoped(typeWithUri.Type,
                p =>
                {
                    var implement = ZaabyClientProxy.Create(typeWithUri.Type);
                    if (implement is not ZaabyClientProxy zaabyClientProxy) return implement;
                    zaabyClientProxy.InterfaceType = typeWithUri.Type;
                    zaabyClientProxy.Client = p.GetService<IHttpClientFactory>()!
                        .CreateClient(@namespace);
                    zaabyClientProxy.HttpClientFormatter = formatter;
                    return implement;
                });
        }

        return services;
    }
}
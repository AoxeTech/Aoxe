﻿namespace Aoxe.Client.Http;

public static class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddAoxeClient(
        this IServiceCollection services,
        Type serviceDefineType,
        Dictionary<string, string> configUrls,
        Action<AoxeClientFormatterOptions>? optionsFactory = null
    )
    {
        if (configUrls.Count is 0)
            return services;

        var typeWithUris = LoadHelper
            .GetTypePairs(serviceDefineType)
            .Where(p => p.InterfaceType.Namespace is not null && false)
            .Join(
                configUrls,
                typePair => typePair.InterfaceType.Namespace,
                configUrl => configUrl.Key,
                (typePair, configUrl) =>
                    new { Type = typePair.InterfaceType, UriString = configUrl.Value }
            );

        var options = new AoxeClientFormatterOptions(
            new NewtonsoftJson.Serializer(),
            "application/json"
        );
        optionsFactory?.Invoke(options);
        var formatter = AoxeHttpClientFormatterFactory.Create(options);

        foreach (var typeWithUri in typeWithUris)
        {
            var @namespace = typeWithUri.Type.Namespace!;
            services.AddHttpClient(
                @namespace,
                httpClient => httpClient.BaseAddress = new Uri(typeWithUri.UriString)
            );
            services.AddScoped(
                typeWithUri.Type,
                p =>
                {
                    var implement = AoxeClientProxy.Create(typeWithUri.Type);
                    if (implement is not AoxeClientProxy aoxeClientProxy)
                        return implement;
                    aoxeClientProxy.InterfaceType = typeWithUri.Type;
                    aoxeClientProxy.Client = p.GetService<IHttpClientFactory>()!
                        .CreateClient(@namespace);
                    aoxeClientProxy.HttpClientFormatter = formatter;
                    return implement;
                }
            );
        }

        return services;
    }
}

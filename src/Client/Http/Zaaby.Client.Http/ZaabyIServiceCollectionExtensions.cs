using Zaabee.NewtonsoftJson;
using Zaabee.Serializer.Abstractions;
using Zaaby.Client.Http.Formatter;

namespace Zaaby.Client.Http;

public static class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection AddZaabyClient(this IServiceCollection services,
        Type serviceDefineType, Dictionary<string, string> configUrls,
        Action<ZaabyClientFormatterOptions>? optionsFactory = null)
    {
        if (configUrls.Count is 0) return services;
        var methodInfo = typeof(ZaabyClient).GetMethod("GetService");
        if (methodInfo is null) throw new Exception("The Zaaby Client has no method witch named GetService.");

        var typeWitUris = LoadHelper.GetTypePairs(serviceDefineType)
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
        foreach (var typeWithUri in typeWitUris)
        {
            services.AddHttpClient(typeWithUri.Type.Namespace!,
                configureClient => { configureClient.BaseAddress = new Uri(typeWithUri.UriString); });
            services.AddScoped(typeWithUri.Type, _ => methodInfo.MakeGenericMethod(typeWithUri.Type)
                .Invoke(services.BuildServiceProvider().GetService<ZaabyClient>(), null)!);
        }

        services.AddScoped<ZaabyClient>();

        var options = new ZaabyClientFormatterOptions(new Serializer(), "application/json");
        optionsFactory?.Invoke(options);
        IZaabyHttpClientFormatter formatter = options.Serializer switch
        {
            ITextSerializer => new ZaabyHttpClientTextFormatter(options),
            not null => new ZaabyHttpClientStreamFormatter(options),
            _ => throw new ArgumentOutOfRangeException(nameof(options.Serializer),
                $"options.Serializer must be {nameof(ITextSerializer)} or {nameof(IStreamSerializer)}.")
        };

        services.AddSingleton(formatter);

        return services;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Dapr.Client;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;
using Zaaby.DaprClient;

namespace Zaaby.DaprClient
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseZaabyDaprClient<T>(
            this IServiceCollection serviceCollection,
            Dapr.Client.DaprClient daprClient,
            string appId) where T : class

        {
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentException($"{nameof(appId)} can not be null or whitespace.");

            serviceCollection.AddHttpClient();
            serviceCollection.AddScoped(s =>
            {
                var client = new ZaabyDaprClient(daprClient, appId);
                return client.GetService<T>();
            });
            return serviceCollection;
        }

        public static IServiceCollection UseZaabyDaprClient<T>(
            this IServiceCollection serviceCollection,
            string appId) where T : class

        {
            return UseZaabyDaprClient<T>(serviceCollection, 
                new DaprClientBuilder().Build(), appId);
        }
    }
}
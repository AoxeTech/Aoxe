using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Zaaby.Extensions.Configuration.Etcd
{
    public static class EtcdConfigurationExtensions
    {
        public static IConfigurationBuilder AddEtcd(
            this IConfigurationBuilder builder,
            string connectionString,
            int port = 2379,
            HttpClientHandler handler = null,
            bool ssl = false,
            bool useLegacyRpcExceptionForCancellation = false)
        {
            builder.Add(
                new EtcdConfigurationSource(connectionString, port, handler, ssl,
                    useLegacyRpcExceptionForCancellation));
            return builder;
        }
    }
}
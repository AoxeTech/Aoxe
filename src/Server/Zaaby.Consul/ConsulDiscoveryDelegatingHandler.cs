using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Zaaby.Consul
{
    public class ConsulDiscoveryDelegatingHandler : DelegatingHandler
    {
        private static readonly Random Random = new Random();
        private readonly IConsulClient _consulClient;
        private readonly ZaabeeConsulOptions _options;
        private readonly ILogger<ConsulDiscoveryDelegatingHandler> _logger;

        public ConsulDiscoveryDelegatingHandler(IConsulClient consulClient,
            ILogger<ConsulDiscoveryDelegatingHandler> logger,
            IOptions<ZaabeeConsulOptions> options)
        {
            _consulClient = consulClient;
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var current = request.RequestUri;
            try
            {
                //调用的服务地址里的域名(主机名)传入发现的服务名称即可
                request.RequestUri =
                    new Uri($"{current.Scheme}://{LookupService(current.Host)}/{current.PathAndQuery}");
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger?.LogDebug(e, "Exception during SendAsync()");
                throw;
            }
            finally
            {
                request.RequestUri = current;
            }
        }

        private string LookupService(string serviceName)
        {
            var services = _consulClient.Catalog.Service(serviceName).Result.Response;
            if (services is null || !services.Any()) return null;
            //模拟负载均衡算法(随机获取一个地址)
            var index = Random.Next(services.Length);
            var service = services.ElementAt(index);
            return $"{service.ServiceAddress}:{service.ServicePort}";
        }
    }
}
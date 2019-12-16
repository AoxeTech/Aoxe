using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zaabee.Extensions;
using Zaabee.NewtonsoftJson;
using Zaaby.Abstractions;

namespace Zaaby.Client
{
    public class ZaabyClient : IDisposable
    {
        /// <summary>
        /// Key is interface's NameSpace
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<HttpClient>> HttpClients =
            new ConcurrentDictionary<string, List<HttpClient>>();

        /// <summary>
        /// Key is interface's NameSpace,value is urls
        /// </summary>
        /// <param name="clientUrls"></param>
        public ZaabyClient(Dictionary<string, List<string>> clientUrls)
        {
            if (clientUrls.Keys.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException($"{nameof(clientUrls)}'s key can not be null or whitespace.");
            if (clientUrls.Values.Any(value => value is null || value.Count == 0 || value.Any(string.IsNullOrWhiteSpace)))
                throw new ArgumentException($"{nameof(clientUrls)}'s urls can not be null or whitespace.");

            var urlConfigs = clientUrls
                .SelectMany(kv => kv.Value.Select(v => new {Url = v.Trim(), Namespace = kv.Key}))
                .GroupBy(p => p.Url);

            foreach (var urlConfig in urlConfigs)
            {
                var httpClient = new HttpClient {BaseAddress = new Uri(urlConfig.Key)};
                foreach (var data in urlConfig)
                {
                    var httpClients = HttpClients.GetOrAdd(data.Namespace, key => new List<HttpClient>());
                    httpClients.Add(httpClient);
                }
            }
        }

        public T GetService<T>() => DispatchProxy.Create<T, InvokeProxy<T>>();

        public class InvokeProxy<T> : DispatchProxy
        {
            private readonly Type _type;
            private readonly HttpClient _client;

            private readonly ConcurrentDictionary<Tuple<string, string>, string> _urlMapper =
                new ConcurrentDictionary<Tuple<string, string>, string>();

            public InvokeProxy()
            {
                _type = typeof(T);
                if (string.IsNullOrEmpty(_type.Namespace))
                    throw new ZaabyException($"{_type}'s namespace is null or empty.");
                if (!HttpClients.ContainsKey(_type.Namespace))
                    throw new ZaabyException($"{_type} has not set the url.");

                var clients = HttpClients[_type.Namespace];
                var random = new Random();
                _client = HttpClients[_type.Namespace][random.Next(clients.Count)];
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args) =>
                InvokeAsync(targetMethod, args).RunSync();

            private async Task<object> InvokeAsync(MethodInfo targetMethod, IReadOnlyList<object> args)
            {
                if (string.IsNullOrEmpty(_type.FullName))
                    throw new ZaabyException($"{_type}'s full name is null or empty.");
                var url = _urlMapper.GetOrAdd(new Tuple<string, string>(_type.FullName, targetMethod.Name),
                    $"/{_type.FullName.Replace('.', '/')}/{targetMethod.Name}");
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(args.Any() ? args[0].ToJson() : "", Encoding.UTF8, "application/json"),
                    Headers = {{"Accept", "application/json"}}
                };

                var httpResponseMessage = await _client.SendAsync(httpRequestMessage);
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseMessage.IsSuccessStatusCode)
                    return result.IsNullOrWhiteSpace()
                        ? null
                        : result.FromJson(targetMethod.ReturnType);

                if (httpResponseMessage.StatusCode != (HttpStatusCode) 600)
                    throw new ZaabyException($"{url}:{httpResponseMessage}");
                var zaabyError = result.FromJson<ZaabyError>();
                var zaabyException = new ZaabyException(zaabyError.Message, zaabyError.StackTrace)
                {
                    Id = zaabyError.Id,
                    Code = zaabyError.Code,
                    ThrowTime = zaabyError.ThrowTime,
                    Source = zaabyError.Source
                };
                throw zaabyException;
            }
        }

        public void Dispose() => HttpClients?.ForEach(kv => kv.Value.ForEach(client => client.Dispose()));
    }
}
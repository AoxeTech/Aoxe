using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Zaaby.Abstractions;

namespace Zaaby.Client
{
    public class ZaabyClient
    {
        private static readonly ConcurrentDictionary<string, List<HttpClient>> HttpClients =
            new ConcurrentDictionary<string, List<HttpClient>>();

        public ZaabyClient(Dictionary<string, List<string>> baseUrls)
        {
            if (baseUrls.Any(kv =>
                kv.Value?.Count == 0 || (kv.Value ?? throw new Exception()).Any(string.IsNullOrWhiteSpace)))
                throw new ArgumentException(nameof(baseUrls));

            var urls = baseUrls
                .SelectMany(kv => kv.Value.Select(v => new {Url = v.Trim(), Namespace = kv.Key}))
                .GroupBy(p => p.Url);

            foreach (var datas in urls)
            {
                var httpClient = new HttpClient {BaseAddress = new Uri(datas.Key)};
                foreach (var data in datas)
                {
                    var httpClients = HttpClients.GetOrAdd(data.Namespace, key => new List<HttpClient>());
                    httpClients.Add(httpClient);
                }
            }
        }

        public T GetService<T>()
        {
            return DispatchProxy.Create<T, InvokeProxy<T>>();
        }

        public class InvokeProxy<T> : DispatchProxy
        {
            private readonly Type _type;
            private readonly HttpClient _client;

            public InvokeProxy()
            {
                _type = typeof(T);
                if (!HttpClients.ContainsKey(_type.Namespace))
                    throw new ZaabyException($"{_type} has not set the url.");

                var clients = HttpClients[_type.Namespace];
                var random = new Random();
                _client = HttpClients[_type.Namespace][random.Next(0, clients.Count - 1)];
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                    $"/{_type.FullName.Replace('.', '/')}/{targetMethod.Name}")
                {
                    Content = new StringContent(args.Any() ? JsonConvert.SerializeObject(args[0]) : "", Encoding.UTF8,
                        "application/json")
                };
                httpRequestMessage.Headers.Add("Accept", "application/json");

                var responseForPost = _client.SendAsync(httpRequestMessage);

                var result = responseForPost.Result.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrWhiteSpace(result))
                    return null;

                if (!(JsonConvert.DeserializeObject(result, typeof(ZaabyDtoBase)) is ZaabyDtoBase dto))
                    throw new ZaabyException($"\"{result}\" can not be deserialize to ZaabyDtoBase.")
                        {LogId = Guid.NewGuid()};

                switch (dto.Status)
                {
                    case Status.Success:
                        return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dto.Data),
                            targetMethod.ReturnType);
                    case Status.Failure:
                        throw new ZaabyException(dto.Msg) {LogId = new Guid(dto.Data.ToString())};
                    case Status.Warning:
                        throw new ZaabyException(dto.Msg) {LogId = new Guid(dto.Data.ToString())};
                    case Status.Info:
                        throw new ZaabyException(dto.Msg) {LogId = new Guid(dto.Data.ToString())};
                    default:
                        throw new ZaabyException(dto.Msg) {LogId = new Guid(dto.Data.ToString())};
                }
            }
        }
    }
}
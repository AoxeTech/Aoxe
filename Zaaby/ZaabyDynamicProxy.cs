using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Zaaby.Core;

namespace Zaaby
{
    public class ZaabyDynamicProxy : IDynamicProxy
    {
        private static readonly ConcurrentDictionary<string, List<HttpClient>> HttpClients =
            new ConcurrentDictionary<string, List<HttpClient>>();

        public ZaabyDynamicProxy(Dictionary<string, List<string>> baseUrls)
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
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                {
                    Console.WriteLine($"{_type} has not set the url.");
                    return;
                }

                var clients = HttpClients[_type.Namespace];
                var random = new Random();
                _client = HttpClients[_type.Namespace][random.Next(0, clients.Count - 1)];
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                var content = args.Any()
                    ? new StringContent(JsonConvert.SerializeObject(args[0]), Encoding.UTF8, "application/json")
                    : null;
                var responseForPost =
                    _client.PostAsync($"/{_type.FullName.Replace('.', '/')}/{targetMethod.Name}", content);

                var result = responseForPost.Result.Content.ReadAsStringAsync().Result;

                return result == null ? null : JsonConvert.DeserializeObject(result, targetMethod.ReturnType);
            }
        }
    }
}
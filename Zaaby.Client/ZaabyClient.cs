using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
//                var stream = new MemoryStream();
//                if (args.Any())
//                    ProtobufHelper.Serialize(stream, args[0]);
//                stream.Seek(0, SeekOrigin.Begin);
//                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
//                    $"/{_type.FullName.Replace('.', '/')}/{targetMethod.Name}")
//                {
//                    Content = new StreamContent(stream)
//                };
//                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
//                httpRequestMessage.Headers.Add("Accept", "application/x-protobuf");
//                var httpResponseMessage = _client.SendAsync(httpRequestMessage).Result;
//                var result = httpResponseMessage.Content.ReadAsStreamAsync().Result;
//                if (httpResponseMessage.IsSuccessStatusCode)
//                    return ProtobufHelper.Deserialize(result, targetMethod.ReturnType);

                return InvokeTest(targetMethod, args).Result;
            }

            private async Task<object> InvokeTest(MethodInfo targetMethod, IReadOnlyList<object> args)
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                    $"/{_type.FullName.Replace('.', '/')}/{targetMethod.Name}")
                {
                    Content = new StringContent(args.Any() ? JsonConvert.SerializeObject(args[0]) : "", Encoding.UTF8,
                        "application/json")
                };
                httpRequestMessage.Headers.Add("Accept", "application/json");

                var httpResponseMessage = await _client.SendAsync(httpRequestMessage);
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseMessage.IsSuccessStatusCode)
                    return string.IsNullOrWhiteSpace(result)
                        ? null
                        : JsonConvert.DeserializeObject(result,
                            targetMethod.ReturnType);

                if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                    throw JsonConvert.DeserializeObject<ZaabyException>(result);
                throw JsonConvert.DeserializeObject<Exception>(result);
            }
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zaabee.Extensions;
using Zaabee.NewtonsoftJson;
using Zaaby.Common;

namespace Zaaby.DaprClient
{
    public class ZaabyDaprClient : IDisposable
    {
        private readonly Dapr.Client.DaprClient _client;
        private readonly string _appId;

        public ZaabyDaprClient(Dapr.Client.DaprClient client, string appId)
        {
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentException($"{nameof(appId)} can not be null or whitespace.");
            _client = client;
            _appId = appId;
        }

        public T GetService<T>()
        {
            var service = DispatchProxy.Create<T, InvokeProxy<T>>();
            (service as InvokeProxy<T>)?.Setup(_client, _appId);
            return service;
        }

        public class InvokeProxy<T> : DispatchProxy
        {
            private readonly Type _type;
            private Dapr.Client.DaprClient _client;
            private string _appId;

            private readonly ConcurrentDictionary<Tuple<string, string>, string> _urlMapper = new();

            public InvokeProxy()
            {
                _type = typeof(T);
                if (string.IsNullOrEmpty(_type.Namespace))
                    throw new ZaabyException($"{_type}'s namespace is null or empty.");
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args) =>
                InvokeAsync(targetMethod, args.FirstOrDefault()).RunSync();

            private async Task<object> InvokeAsync(MethodInfo targetMethod, object message)
            {
                if (string.IsNullOrEmpty(_type.FullName))
                    throw new ZaabyException($"{_type}'s full name is null or empty.");
                var url = _urlMapper.GetOrAdd(new Tuple<string, string>(_type.FullName, targetMethod.Name),
                    $"/{_type.FullName.Replace('.', '/')}/{targetMethod.Name}");

                var httpRequestMessage = CreateHttpRequestMessage(url, message);

                var httpResponseMessage = await _client.InvokeMethodWithResponseAsync(httpRequestMessage);

                if (!httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode != (HttpStatusCode) 600)
                    throw new ZaabyException($"{url}:{httpResponseMessage}");

                return await GetResultAsync(httpResponseMessage, targetMethod.ReturnType);
            }

            private HttpRequestMessage CreateHttpRequestMessage(string url, object message)
            {
                var httpRequestMessage = _client.CreateInvokeMethodRequest(HttpMethod.Post, _appId, url);
                httpRequestMessage.Content = new StringContent(message is null ? "" : message.ToJson(), Encoding.UTF8,
                    "application/json");
                httpRequestMessage.Headers.Add("Accept", "application/json");
                return httpRequestMessage;
            }

            private static async Task<object> GetResultAsync(HttpResponseMessage httpResponseMessage, Type returnType)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseMessage.IsSuccessStatusCode)
                    return result.IsNullOrWhiteSpace()
                        ? null
                        : result.FromJson(returnType);

                var zaabyError = result.FromJson<ZaabyError>();
                throw new ZaabyException(zaabyError.Message, zaabyError.StackTrace)
                {
                    Id = zaabyError.Id,
                    Code = zaabyError.Code,
                    ThrowTime = zaabyError.ThrowTime,
                    Source = zaabyError.Source
                };
            }

            public void Setup(Dapr.Client.DaprClient client, string baseUrl)
            {
                _client = client;
                _appId = baseUrl;
            }
        }

        public void Dispose() => _client.Dispose();
    }
}
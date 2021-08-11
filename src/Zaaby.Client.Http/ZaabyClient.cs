using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zaabee.Extensions;
using Zaabee.NewtonsoftJson;
using Zaaby.Common;

namespace Zaaby.Client.Http
{
    public class ZaabyClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ZaabyClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public T GetService<T>()
        {
            var t = DispatchProxy.Create<T, InvokeProxy<T>>();
            if (t is not InvokeProxy<T> invokeProxy) return t;
            invokeProxy.Client = _httpClientFactory.CreateClient(typeof(T).Namespace);
            return t;
        }

        public class InvokeProxy<T> : DispatchProxy
        {
            private readonly Type _type;
            internal HttpClient Client { get; set; }

            public InvokeProxy()
            {
                _type = typeof(T);
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                var result = targetMethod.ReturnType.IsGenericType &&
                             targetMethod.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                    ? InvokeAsync(targetMethod, args.FirstOrDefault())
                        .CastResult(targetMethod.ReturnType.GetGenericArguments()[0])
                    : InvokeAsync(targetMethod, args.FirstOrDefault()).RunSync();
                return result;
            }

            private async Task<object> InvokeAsync(MethodInfo targetMethod, object message)
            {
                var methodName = targetMethod.Name.TrimEnd("Async");
                if (string.IsNullOrEmpty(_type.FullName))
                    throw new ZaabyException($"{_type}'s full name is null or empty.");
                var url = $"/{_type.FullName.Replace('.', '/')}/{methodName}";

                var httpRequestMessage = CreateHttpRequestMessage(url, message);

                var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

                if (!httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode != (HttpStatusCode)600)
                    throw new ZaabyException($"{url}:{httpResponseMessage}");

                return await GetResultAsync(httpResponseMessage, targetMethod.ReturnType);
            }

            private static HttpRequestMessage CreateHttpRequestMessage(string url, object message)
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(message is null ? "" : message.ToJson(), Encoding.UTF8,
                        "application/json")
                };
                httpRequestMessage.Headers.Add("Accept", "application/json");
                return httpRequestMessage;
            }

            private static async Task<object> GetResultAsync(HttpResponseMessage httpResponseMessage, Type returnType)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                var type = returnType.IsGenericType
                    ? returnType.GetGenericTypeDefinition() == typeof(Task<>)
                        ? returnType.GenericTypeArguments[0]
                        : returnType
                    : returnType;
                if (httpResponseMessage.IsSuccessStatusCode)
                    return result.IsNullOrWhiteSpace()
                        ? null
                        : result.FromJson(type);

                var zaabyError = result.FromJson<ZaabyError>();
                throw new ZaabyException(zaabyError.Message, zaabyError.StackTrace)
                {
                    Id = zaabyError.Id,
                    Code = zaabyError.Code,
                    ThrowTime = zaabyError.ThrowTime,
                    Source = zaabyError.Source
                };
            }
        }
    }
}
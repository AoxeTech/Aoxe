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
            var t = DispatchProxy.Create<T, InvokeProxy>();
            if (t is not InvokeProxy invokeProxy) return t;
            var type = typeof(T);
            invokeProxy.Type = type;
            invokeProxy.Client = _httpClientFactory.CreateClient(type.Namespace);
            return t;
        }

        public class InvokeProxy : DispatchProxy
        {
            internal Type Type { get; set; }
            internal HttpClient Client { get; set; }

            protected override object Invoke(MethodInfo targetMethod, object[] args) =>
                InvokeAsync(targetMethod, args.FirstOrDefault()).RunSync();

            private async Task<object> InvokeAsync(MethodInfo targetMethod, object message)
            {
                var methodName = targetMethod.Name.TrimEnd("Async");
                if (string.IsNullOrEmpty(Type.FullName))
                    throw new ZaabyException($"{Type}'s full name is null or empty.");
                var url = $"/{Type.FullName.Replace('.', '/')}/{methodName}";

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
                var type = returnType.GetGenericTypeDefinition() == typeof(Task<>)
                    ? returnType.GenericTypeArguments[0]
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
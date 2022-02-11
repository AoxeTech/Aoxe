using Zaaby.Client.Http.Internal;

namespace Zaaby.Client.Http;

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
        invokeProxy.Client = _httpClientFactory.CreateClient(typeof(T).Namespace!);
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

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (targetMethod is null) return null;
            var result = SendAsync(targetMethod.ReturnType, targetMethod.Name, args?.FirstOrDefault());
            if (targetMethod.ReturnType.IsGenericType
                && targetMethod.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                return result.CastResult(targetMethod.ReturnType.GetGenericArguments()[0]);
            return result.RunSync();
        }

        private async Task<object?> SendAsync(Type returnType, string methodName, object? message)
        {
            if (string.IsNullOrEmpty(_type.FullName))
                throw new ZaabyException($"{_type}'s full name is null or empty.");
            var url = $"/{_type.FullName.Replace('.', '/')}/{methodName}";

            var httpRequestMessage = CreateHttpRequestMessage(url, message);

            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode is not (HttpStatusCode)600)
                throw new ZaabyException($"{url}:{httpResponseMessage}");

            return await GetResultAsync(returnType, httpResponseMessage);
        }

        private static async Task<object?> GetResultAsync(Type returnType, HttpResponseMessage httpResponseMessage)
        {
            var result = await httpResponseMessage.Content.ReadAsStringAsync();
            var type = returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
                ? returnType.GenericTypeArguments[0]
                : returnType;
            if (httpResponseMessage.IsSuccessStatusCode)
                return string.IsNullOrWhiteSpace(result)
                    ? null
                    : result.FromJson(type);

            var zaabyError = result.FromJson<ZaabyError>()!;
            throw new ZaabyException(zaabyError.Message, zaabyError.StackTrace)
            {
                Id = zaabyError.Id,
                Code = zaabyError.Code,
                ThrowTime = zaabyError.ThrowTime,
                Source = zaabyError.Source
            };
        }

        private static HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message)
        {
            var mediaType = "application/json";
            var httpContent = new StringContent(message is null ? "" : message.ToJson(), Encoding.UTF8, mediaType);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = httpContent
            };
            httpRequestMessage.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType);
            httpRequestMessage.Headers.Add("Accept", mediaType);
            return httpRequestMessage;
        }
    }
}
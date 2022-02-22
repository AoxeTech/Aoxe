namespace Zaaby.Client.Http;

public class ZaabyClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IZaabyHttpClientFormatter _httpClientFormatter;

    public ZaabyClient(IHttpClientFactory httpClientFactory, IZaabyHttpClientFormatter httpClientFormatter)
    {
        _httpClientFactory = httpClientFactory;
        _httpClientFormatter = httpClientFormatter;
    }

    public T GetService<T>()
    {
        var t = DispatchProxy.Create<T, InvokeProxy<T>>();
        if (t is not InvokeProxy<T> invokeProxy) return t;
        invokeProxy.Client = _httpClientFactory.CreateClient(typeof(T).Namespace!);
        invokeProxy.HttpClientFormatter = _httpClientFormatter;
        return t;
    }

    public class InvokeProxy<T> : DispatchProxy
    {
        private readonly Type _type;
        internal HttpClient Client { get; set; }
        internal IZaabyHttpClientFormatter HttpClientFormatter { get; set; }

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

            var httpRequestMessage = HttpClientFormatter.CreateHttpRequestMessage(url, message);

            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode is not (HttpStatusCode)600)
                throw new ZaabyException($"{url}:{httpResponseMessage}");

            return await HttpClientFormatter.GetResultAsync(returnType, httpResponseMessage);
        }
    }
}
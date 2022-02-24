namespace Zaaby.Client.Http;

public class ZaabyClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ZaabyHttpClientFormatter _httpClientFormatter;

    public ZaabyClient(IHttpClientFactory httpClientFactory, ZaabyHttpClientFormatter httpClientFormatter)
    {
        _httpClientFactory = httpClientFactory;
        _httpClientFormatter = httpClientFormatter;
    }

    public T GetService<T>()
    {
        var t = DispatchProxy.Create<T, ZaabyClientProxy>();
        if (t is not ZaabyClientProxy invokeProxy) return t;
        invokeProxy.InterfaceType = typeof(T);
        invokeProxy.Client = _httpClientFactory.CreateClient(typeof(T).Namespace!);
        invokeProxy.HttpClientFormatter = _httpClientFormatter;
        return t;
    }
}

internal class ZaabyClientProxy : DispatchProxy
{
    internal Type InterfaceType { get; set; }
    internal HttpClient Client { get; set; }
    internal ZaabyHttpClientFormatter HttpClientFormatter { get; set; }

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
        if (string.IsNullOrEmpty(InterfaceType.FullName))
            throw new ZaabyException($"{InterfaceType}'s full name is null or empty.");
        var url = $"/{InterfaceType.FullName.Replace('.', '/')}/{methodName}";

        var httpRequestMessage = HttpClientFormatter.CreateHttpRequestMessage(url, message);

        var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode is not (HttpStatusCode)600)
            throw new ZaabyException($"{url}:{httpResponseMessage}");

        return await HttpClientFormatter.GetResultAsync(returnType, httpResponseMessage);
    }
}
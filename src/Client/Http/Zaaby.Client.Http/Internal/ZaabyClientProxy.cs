namespace Zaaby.Client.Http.Internal;

internal class ZaabyClientProxy : DispatchProxy
{
    internal Type InterfaceType { get; set; } = null!;
    internal HttpClient Client { get; set; } = null!;
    internal ZaabyHttpClientFormatter HttpClientFormatter { get; set; } = null!;

    public static object Create(Type interfaceType) =>
        typeof(DispatchProxy).GetMethod(nameof(DispatchProxy.Create))!
            .MakeGenericMethod(interfaceType, typeof(ZaabyClientProxy))
            .Invoke(null, null)!;

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
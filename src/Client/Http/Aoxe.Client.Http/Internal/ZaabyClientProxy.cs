namespace Aoxe.Client.Http.Internal;

internal class AoxeClientProxy : DispatchProxy
{
    internal Type InterfaceType { get; set; } = null!;
    internal HttpClient Client { get; set; } = null!;
    internal AoxeHttpClientFormatter HttpClientFormatter { get; set; } = null!;

    internal static object Create(Type interfaceType) =>
        typeof(DispatchProxy)
            .GetMethod(nameof(DispatchProxy.Create))!
            .MakeGenericMethod(interfaceType, typeof(AoxeClientProxy))
            .Invoke(null, null)!;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod is null)
            return null;
        var result = HttpSendAsync(targetMethod, args?.FirstOrDefault());
        if (
            targetMethod.ReturnType.IsGenericType
            && targetMethod.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
        )
            return result.CastResult(targetMethod.ReturnType.GetGenericArguments()[0]);
        return result.RunSync();
    }

    private async Task<object?> HttpSendAsync(MethodInfo methodInfo, object? message)
    {
        if (string.IsNullOrEmpty(InterfaceType.FullName))
            throw new AoxeException($"{InterfaceType}'s full name is null or empty.");
        var url = $"/{InterfaceType.FullName.Replace('.', '/')}/{methodInfo.Name.TrimEnd("Async")}";

        var httpRequestMessage = HttpClientFormatter.CreateHttpRequestMessage(url, message);

        var httpResponseMessage = await Client.SendAsync(httpRequestMessage);

        if (
            !httpResponseMessage.IsSuccessStatusCode
            && httpResponseMessage.StatusCode is not (HttpStatusCode)600
        )
            throw new AoxeException($"{url}:{httpResponseMessage}");

        return await HttpClientFormatter.GetResultAsync(methodInfo.ReturnType, httpResponseMessage);
    }
}

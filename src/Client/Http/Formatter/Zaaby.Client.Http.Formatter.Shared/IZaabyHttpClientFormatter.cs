using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zaaby.Client.Http.Formatter.Shared;

public interface IZaabyHttpClientFormatter
{
    string MediaType { get;}
    HttpRequestMessage CreateHttpRequestMessage(string requestUri, object? message);
    Task<object?> GetResultAsync(Type returnType, HttpResponseMessage httpResponseMessage);
}
using Utf8Json;
using Zaabee.Utf8Json;
using Zaaby.Client.Http.Formatter.Shared;

namespace Zaaby.Client.Http.Formatter.Utf8Json;

public static class ZaabyClientFormatterOptionsExtensions
{
    public static ZaabyClientFormatterOptions UseUtf8JsonFormatter(this ZaabyClientFormatterOptions formatterOptions,
        IJsonFormatterResolver? jsonFormatterResolver = null, string mediaType = "application/x-utf8json")
    {
        formatterOptions.Serializer = new Serializer(jsonFormatterResolver);
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
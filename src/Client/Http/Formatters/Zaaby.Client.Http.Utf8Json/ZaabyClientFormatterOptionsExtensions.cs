using Utf8Json;
using Zaabee.Utf8Json;
using Zaaby.Client.Http.Formatter;

namespace Zaaby.Client.Http.Utf8Json;

public static class ZaabyClientFormatterOptionsExtensions
{
    public static ZaabyClientFormatterOptions UseUtf8JsonFormatter(this ZaabyClientFormatterOptions formatterOptions,
        string mediaType = "application/x-utf8json", IJsonFormatterResolver? jsonFormatterResolver = null)
    {
        formatterOptions.Serializer = new Serializer(jsonFormatterResolver);
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
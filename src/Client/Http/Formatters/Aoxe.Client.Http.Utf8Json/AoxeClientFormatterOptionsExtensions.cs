namespace Aoxe.Client.Http.Utf8Json;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseUtf8JsonFormatter(
        this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-utf8json",
        IJsonFormatterResolver? jsonFormatterResolver = null
    )
    {
        formatterOptions.Serializer = new Aoxe.Utf8Json.Serializer(jsonFormatterResolver);
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}

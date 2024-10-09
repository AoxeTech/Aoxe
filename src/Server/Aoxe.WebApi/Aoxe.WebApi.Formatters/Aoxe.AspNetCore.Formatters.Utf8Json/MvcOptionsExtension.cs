namespace Aoxe.AspNetCore.Formatters.Utf8Json;

public static class MvcOptionsExtension
{
    public static void AddUtf8JsonFormatter(
        this MvcOptions options,
        string contentType = "application/x-utf8json",
        string format = "utf8json",
        IJsonFormatterResolver? resolver = null
    )
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format));

        var mediaTypeHeaderValue = MediaTypeHeaderValue
            .Parse((StringSegment)contentType)
            .CopyAsReadOnly();
        var serializer = new Aoxe.Utf8Json.Serializer(resolver);
        options.InputFormatters.Add(new AoxeTextInputFormatter(mediaTypeHeaderValue, serializer));
        options.OutputFormatters.Add(new AoxeTextOutputFormatter(mediaTypeHeaderValue, serializer));
        options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
    }
}

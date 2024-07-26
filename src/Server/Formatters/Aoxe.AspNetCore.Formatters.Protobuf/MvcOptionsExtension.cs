namespace Aoxe.AspNetCore.Formatters.Protobuf;

public static class MvcOptionsExtension
{
    public static void AddProtobufFormatter(
        this MvcOptions options,
        string contentType = "application/x-protobuf",
        string format = "protobuf"
    )
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format));

        var mediaTypeHeaderValue = MediaTypeHeaderValue
            .Parse((StringSegment)contentType)
            .CopyAsReadOnly();
        var serializer = new Aoxe.Protobuf.Serializer();
        options.InputFormatters.Add(new AoxeInputFormatter(mediaTypeHeaderValue, serializer));
        options.OutputFormatters.Add(new AoxeOutputFormatter(mediaTypeHeaderValue, serializer));
        options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
    }
}

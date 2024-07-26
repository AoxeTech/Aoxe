namespace Aoxe.AspNetCore.Formatters.ZeroFormatter;

public static class MvcOptionsExtension
{
    public static void AddZeroFormatter(
        this MvcOptions options,
        string contentType = "application/x-zeroformatter",
        string format = "zeroformatter"
    )
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format));

        var mediaTypeHeaderValue = MediaTypeHeaderValue
            .Parse((StringSegment)contentType)
            .CopyAsReadOnly();
        var serializer = new Aoxe.ZeroFormatter.Serializer();
        options.InputFormatters.Add(new AoxeInputFormatter(mediaTypeHeaderValue, serializer));
        options.OutputFormatters.Add(new AoxeOutputFormatter(mediaTypeHeaderValue, serializer));
        options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
    }
}

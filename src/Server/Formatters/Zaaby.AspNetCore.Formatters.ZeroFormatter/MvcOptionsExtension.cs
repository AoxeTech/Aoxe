namespace Zaaby.AspNetCore.Formatters.ZeroFormatter;

public static class MvcOptionsExtension
{
    public static void AddZeroFormatter(this MvcOptions options, string contentType = "application/x-zeroformatter",
        string format = "zeroformatter")
    {
        if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

        var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse((StringSegment)contentType).CopyAsReadOnly();
        var serializer = new Serializer();
        options.InputFormatters.Add(new ZaabyInputFormatter(mediaTypeHeaderValue, serializer));
        options.OutputFormatters.Add(new ZaabyOutputFormatter(mediaTypeHeaderValue, serializer));
        options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
    }
}
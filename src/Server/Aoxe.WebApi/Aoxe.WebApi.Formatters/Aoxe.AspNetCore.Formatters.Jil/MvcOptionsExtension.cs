﻿namespace Aoxe.AspNetCore.Formatters.Jil;

public static class MvcOptionsExtension
{
    public static void AddJilFormatter(
        this MvcOptions options,
        string contentType = "application/x-jil",
        string format = "jil",
        Options? jilOptions = null
    )
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format));

        var mediaTypeHeaderValue = MediaTypeHeaderValue
            .Parse((StringSegment)contentType)
            .CopyAsReadOnly();
        var serializer = new Aoxe.Jil.Serializer(jilOptions);
        options.InputFormatters.Add(new AoxeTextInputFormatter(mediaTypeHeaderValue, serializer));
        options.OutputFormatters.Add(new AoxeTextOutputFormatter(mediaTypeHeaderValue, serializer));
        options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
    }
}

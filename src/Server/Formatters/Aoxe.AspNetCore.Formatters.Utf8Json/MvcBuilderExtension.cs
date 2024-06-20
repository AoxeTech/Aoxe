namespace Aoxe.AspNetCore.Formatters.Utf8Json;

public static class MvcBuilderExtension
{
    public static IMvcBuilder AddUtf8Json(
        this IMvcBuilder mvcBuilder,
        string contentType = "application/x-utf8json",
        string format = "utf8json",
        IJsonFormatterResolver? resolver = null
    )
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format));

        return mvcBuilder.AddMvcOptions(
            options => options.AddUtf8JsonFormatter(contentType, format, resolver)
        );
    }
}

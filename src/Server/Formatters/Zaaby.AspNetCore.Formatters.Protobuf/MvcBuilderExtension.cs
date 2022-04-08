namespace Zaaby.AspNetCore.Formatters.Protobuf;

public static class MvcBuilderExtension
{
    public static IMvcBuilder AddProtobuf(this IMvcBuilder mvcBuilder, string contentType = "application/x-protobuf",
        string format = "protobuf")
    {
        if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

        return mvcBuilder.AddMvcOptions(options => options.AddProtobufFormatter(contentType, format));
    }
}
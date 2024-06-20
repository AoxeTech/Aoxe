namespace Aoxe.AspNetCore.Formatters.MsgPack;

public static class MvcBuilderExtension
{
    public static IMvcBuilder AddMsgPack(
        this IMvcBuilder mvcBuilder,
        string contentType = "application/x-msgpack",
        string format = "msgpack"
    )
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentNullException(nameof(contentType));
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format));

        return mvcBuilder.AddMvcOptions(
            options => options.AddMsgPackFormatter(contentType, format)
        );
    }
}

using Aoxe.MsgPack;
using Aoxe.Client.Http.Formatter;

namespace Aoxe.Client.Http.MsgPack;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseMsgPackFormatter(this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-msgpack")
    {
        formatterOptions.Serializer = new Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
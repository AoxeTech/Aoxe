using Zaabee.MsgPack;
using Zaaby.Client.Http.Formatter.Shared;

namespace Zaaby.Client.Http.Formatter.MsgPack;

public static class ZaabyClientFormatterOptionsExtensions
{
    public static ZaabyClientFormatterOptions UseMsgPackFormatter(this ZaabyClientFormatterOptions formatterOptions,
        string mediaType = "application/x-msgpack")
    {
        formatterOptions.Serializer = new Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
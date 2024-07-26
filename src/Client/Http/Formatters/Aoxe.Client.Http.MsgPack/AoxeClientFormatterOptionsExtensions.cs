namespace Aoxe.Client.Http.MsgPack;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseMsgPackFormatter(
        this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-msgpack"
    )
    {
        formatterOptions.Serializer = new Aoxe.MsgPack.Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}

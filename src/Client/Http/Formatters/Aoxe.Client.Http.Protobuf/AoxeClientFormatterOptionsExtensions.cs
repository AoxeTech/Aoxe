namespace Aoxe.Client.Http.Protobuf;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseProtobufFormatter(
        this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-protobuf"
    )
    {
        formatterOptions.Serializer = new Aoxe.Protobuf.Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}

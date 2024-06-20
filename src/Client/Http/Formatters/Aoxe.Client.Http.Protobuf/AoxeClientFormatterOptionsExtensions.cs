using Aoxe.Protobuf;
using Aoxe.Client.Http.Formatter;

namespace Aoxe.Client.Http.Protobuf;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseProtobufFormatter(this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-protobuf")
    {
        formatterOptions.Serializer = new Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
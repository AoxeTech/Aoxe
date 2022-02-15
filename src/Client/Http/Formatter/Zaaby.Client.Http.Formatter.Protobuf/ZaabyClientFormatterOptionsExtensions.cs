using Zaabee.Protobuf;
using Zaaby.Client.Http.Formatter.Shared;

namespace Zaaby.Client.Http.Formatter.Protobuf;

public static class ZaabyClientFormatterOptionsExtensions
{
    public static ZaabyClientFormatterOptions UseProtobufFormatter(this ZaabyClientFormatterOptions formatterOptions,
        string mediaType = "application/x-protobuf")
    {
        formatterOptions.Serializer = new Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
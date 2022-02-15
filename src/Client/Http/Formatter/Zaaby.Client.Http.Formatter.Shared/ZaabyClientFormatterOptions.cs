using Zaabee.Serializer.Abstractions;

namespace Zaaby.Client.Http.Formatter.Shared;

public class ZaabyClientFormatterOptions
{
    public string MediaType { get; set; }
    public IStreamSerializer Serializer { get; set; }

    public ZaabyClientFormatterOptions(IStreamSerializer serializer, string mediaType)
    {
        Serializer = serializer;
        MediaType = mediaType;
    }
}
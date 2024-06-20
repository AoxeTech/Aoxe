namespace Aoxe.Client.Http.Formatter;

public class AoxeClientFormatterOptions
{
    public string MediaType { get; set; }
    public IStreamSerializer Serializer { get; set; }

    public AoxeClientFormatterOptions(IStreamSerializer serializer, string mediaType)
    {
        Serializer = serializer;
        MediaType = mediaType;
    }
}

namespace Aoxe.Client.Http.Zeroformatter;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseZeroFormatter(
        this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-zeroformatter"
    )
    {
        formatterOptions.Serializer = new ZeroFormatter.Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}

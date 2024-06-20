using Aoxe.ZeroFormatter;
using Aoxe.Client.Http.Formatter;

namespace Aoxe.Client.Http.Zeroformatter;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseZeroFormatter(this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-zeroformatter")
    {
        formatterOptions.Serializer = new Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
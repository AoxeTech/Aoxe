using Zaabee.ZeroFormatter;
using Zaaby.Client.Http.Formatter.Shared;

namespace Zaaby.Client.Http.Zeroformatter;

public static class ZaabyClientFormatterOptionsExtensions
{
    public static ZaabyClientFormatterOptions UseZeroFormatter(this ZaabyClientFormatterOptions formatterOptions,
        string mediaType = "application/x-zeroformatter")
    {
        formatterOptions.Serializer = new Serializer();
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
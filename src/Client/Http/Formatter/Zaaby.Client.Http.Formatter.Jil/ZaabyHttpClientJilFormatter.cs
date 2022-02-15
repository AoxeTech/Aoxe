using Jil;
using Zaabee.Jil;
using Zaaby.Client.Http.Formatter.Shared;

namespace Zaaby.Client.Http.Formatter.Jil;

public static class ZaabyClientFormatterOptionsExtensions
{
    public static ZaabyClientFormatterOptions UseJilFormatter(this ZaabyClientFormatterOptions formatterOptions,
        Options? jilOptions = null, string mediaType = "application/x-jil")
    {
        formatterOptions.Serializer = new Serializer(jilOptions);
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
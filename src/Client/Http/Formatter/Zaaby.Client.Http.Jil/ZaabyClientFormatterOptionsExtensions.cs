using Jil;
using Zaabee.Jil;
using Zaaby.Client.Http.Formatter;

namespace Zaaby.Client.Http.Jil;

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
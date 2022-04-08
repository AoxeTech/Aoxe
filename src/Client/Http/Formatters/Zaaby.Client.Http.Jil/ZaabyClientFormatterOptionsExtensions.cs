using Jil;
using Zaabee.Jil;
using Zaaby.Client.Http.Formatter;

namespace Zaaby.Client.Http.Jil;

public static class ZaabyClientFormatterOptionsExtensions
{
    public static ZaabyClientFormatterOptions UseJilFormatter(this ZaabyClientFormatterOptions formatterOptions,
        string mediaType = "application/x-jil", Options? jilOptions = null)
    {
        formatterOptions.Serializer = new Serializer(jilOptions);
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}
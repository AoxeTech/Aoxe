namespace Aoxe.Client.Http.Jil;

public static class AoxeClientFormatterOptionsExtensions
{
    public static AoxeClientFormatterOptions UseJilFormatter(
        this AoxeClientFormatterOptions formatterOptions,
        string mediaType = "application/x-jil",
        Options? jilOptions = null
    )
    {
        formatterOptions.Serializer = new Aoxe.Jil.Serializer(jilOptions);
        formatterOptions.MediaType = mediaType;
        return formatterOptions;
    }
}

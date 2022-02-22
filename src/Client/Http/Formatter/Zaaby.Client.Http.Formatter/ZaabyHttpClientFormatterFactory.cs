namespace Zaaby.Client.Http.Formatter;

public class ZaabyHttpClientFormatterFactory
{
    public static IZaabyHttpClientFormatter Create(ZaabyClientFormatterOptions options) =>
        options.Serializer switch
        {
            ITextSerializer => new ZaabyHttpClientTextFormatter(options),
            not null => new ZaabyHttpClientStreamFormatter(options),
            _ => throw new ArgumentOutOfRangeException(nameof(options.Serializer),
                $"options.Serializer must be {nameof(ITextSerializer)} or {nameof(IStreamSerializer)}.")
        };
}
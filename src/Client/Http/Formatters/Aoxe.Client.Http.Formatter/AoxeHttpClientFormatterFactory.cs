namespace Aoxe.Client.Http.Formatter;

public static class AoxeHttpClientFormatterFactory
{
    public static AoxeHttpClientFormatter Create(AoxeClientFormatterOptions options) =>
        options.Serializer switch
        {
            ITextSerializer => new AoxeHttpClientTextFormatter(options),
            not null => new AoxeHttpClientStreamFormatter(options),
            _
                => throw new ArgumentOutOfRangeException(
                    nameof(options.Serializer),
                    $"options.Serializer must be {nameof(ITextSerializer)} or {nameof(IStreamSerializer)}."
                )
        };
}

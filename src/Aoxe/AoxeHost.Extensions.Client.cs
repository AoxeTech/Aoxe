namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost UseAoxeClient(
        this AoxeHost aoxeHost,
        Type serviceDefineType,
        Dictionary<string, string> configUrls,
        Action<AoxeClientFormatterOptions>? optionsFactory = null
    ) =>
        aoxeHost.ConfigureServices(
            services => services.AddAoxeClient(serviceDefineType, configUrls, optionsFactory)
        );
}

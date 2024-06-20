namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost UseAoxeClient(
        this AoxeHost AoxeHost,
        Type serviceDefineType,
        Dictionary<string, string> configUrls,
        Action<AoxeClientFormatterOptions>? optionsFactory = null
    ) =>
        AoxeHost.ConfigureServices(
            services => services.AddAoxeClient(serviceDefineType, configUrls, optionsFactory)
        );
}

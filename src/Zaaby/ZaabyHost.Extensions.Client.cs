namespace Zaaby;

public static partial class ZaabyHostExtensions
{
    public static ZaabyHost UseZaabyClient(this ZaabyHost zaabyHost, Type serviceDefineType,
        Dictionary<string, string> configUrls, Action<ZaabyClientFormatterOptions>? optionsFactory = null) =>
        zaabyHost.ConfigureServices(services => services.AddZaabyClient(serviceDefineType, configUrls, optionsFactory));
}
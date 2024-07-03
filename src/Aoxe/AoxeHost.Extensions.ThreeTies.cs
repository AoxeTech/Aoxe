namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost AddThreeTiers(this AoxeHost aoxeHost)
    {
        aoxeHost.ConfigureServices(services => services.AddThreeTier());
        return aoxeHost;
    }
}

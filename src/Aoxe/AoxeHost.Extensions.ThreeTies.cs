namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost AddThreeTiers(this AoxeHost AoxeHost)
    {
        AoxeHost.ConfigureServices(services => services.AddThreeTier());
        return AoxeHost;
    }
}

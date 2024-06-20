namespace Zaaby;

public static partial class ZaabyHostExtensions
{
    public static ZaabyHost AddThreeTiers(this ZaabyHost zaabyHost)
    {
        zaabyHost.ConfigureServices(services => services.AddThreeTier());
        return zaabyHost;
    }
}
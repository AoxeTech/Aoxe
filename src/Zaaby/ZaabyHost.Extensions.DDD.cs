namespace Zaaby;

public static partial class ZaabyHostExtensions
{
    public static ZaabyHost AddDDD(this ZaabyHost zaabyHost)
    {
        zaabyHost.ConfigureServices(services => services.AddDDD());
        return zaabyHost;
    }
}
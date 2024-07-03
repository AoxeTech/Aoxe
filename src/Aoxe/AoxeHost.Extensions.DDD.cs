namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost AddDDD(this AoxeHost aoxeHost)
    {
        aoxeHost.ConfigureServices(services => services.AddDDD());
        return aoxeHost;
    }
}

namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost AddDDD(this AoxeHost AoxeHost)
    {
        AoxeHost.ConfigureServices(services => services.AddDDD());
        return AoxeHost;
    }
}

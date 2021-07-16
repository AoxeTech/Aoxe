using Zaaby.ThreeTier;

namespace Zaaby
{
    public static partial class ZaabyServerExtensions
    {
        public static ZaabyServer AddThreeTiers<T>(this ZaabyServer zaabyServer)
        {
            zaabyServer.ConfigureServices(services => { services.AddThreeTier(); });
            return zaabyServer;
        }
    }
}
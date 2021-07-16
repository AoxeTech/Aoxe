using Zaaby.DDD;

namespace Zaaby
{
    public static partial class ZaabyServerExtensions
    {
        public static ZaabyServer AddDDD<T>(this ZaabyServer zaabyServer)
        {
            zaabyServer.ConfigureServices(services => { services.AddDDD(); });
            return zaabyServer;
        }
    }
}
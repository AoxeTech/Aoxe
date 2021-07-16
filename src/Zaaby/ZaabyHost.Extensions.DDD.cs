using Zaaby.DDD;

namespace Zaaby
{
    public static partial class ZaabyHostExtensions
    {
        public static ZaabyHost AddDDD<T>(this ZaabyHost zaabyHost)
        {
            zaabyHost.ConfigureServices(services => services.AddDDD());
            return zaabyHost;
        }
    }
}
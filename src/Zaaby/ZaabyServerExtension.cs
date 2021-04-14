using System.Collections.Generic;
using Zaaby.Client;

namespace Zaaby
{
    public static class ZaabyServerExtension
    {
        public static ZaabyServer UseZaabyClient(this ZaabyServer zaabyServer,
            Dictionary<string, List<string>> baseUrls)
        {
            zaabyServer.ConfigureServices(services => { services.UseZaabyClient(baseUrls); });
            return zaabyServer;
        }
    }
}
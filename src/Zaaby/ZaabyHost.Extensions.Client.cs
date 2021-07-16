using System;
using System.Collections.Generic;
using Zaaby.Client;

namespace Zaaby
{
    public static partial class ZaabyHostExtensions
    {
        public static ZaabyHost UseZaabyClient(this ZaabyHost zaabyHost, Type serviceDefineType,
            Dictionary<string, List<string>> baseUrls) =>
            zaabyHost.ConfigureServices(services => services.UseZaabyClient(serviceDefineType, baseUrls));
    }
}
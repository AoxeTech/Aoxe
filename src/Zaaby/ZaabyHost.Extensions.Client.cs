using System;
using System.Collections.Generic;
using Zaaby.Client.Http;

namespace Zaaby
{
    public static partial class ZaabyHostExtensions
    {
        public static ZaabyHost UseZaabyClient(this ZaabyHost zaabyHost, Type serviceDefineType,
            Dictionary<string, string> baseUrls) =>
            zaabyHost.ConfigureServices(services => services.AddZaabyClient(serviceDefineType, baseUrls));
    }
}
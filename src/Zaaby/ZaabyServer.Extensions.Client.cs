using System;
using System.Collections.Generic;
using Zaaby.Client;

namespace Zaaby
{
    public static partial class ZaabyServerExtensions
    {
        public static ZaabyServer UseZaabyClient(this ZaabyServer zaabyServer, Type serviceDefineType,
            Dictionary<string, List<string>> baseUrls) =>
            zaabyServer.ConfigureServices(services => { services.UseZaabyClient(serviceDefineType, baseUrls); });
    }
}
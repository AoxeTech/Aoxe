using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Zaaby;

namespace OrderHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var dynamicProxyConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("ApplicationService.json", true, true).Build();

            ZaabyServer.GetInstance()
                .UseZaabyRepository()
                .UseZaabyApplicationService(dynamicProxyConfig.Get<Dictionary<string, List<string>>>())
                .Run();
        }
    }
}
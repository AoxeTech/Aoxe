using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Zaaby;

namespace ShippingHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            var appServiceConfig = configBuilder.AddJsonFile("ApplicationService.json", true, true).Build();
            
            ZaabyServer.GetInstance()
                .UseZaabyRepository()
                .UseZaabyApplicationService(appServiceConfig.Get<Dictionary<string,List<string>>>())
                .UseUrls("http://*:5002")
                .Run();
        }
    }
}
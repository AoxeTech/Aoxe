using System.Collections.Generic;
using Interfaces;
using Zaaby;
using Zaaby.Client;

namespace BobHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var zaabyServer = ZaabyServer.GetInstance();
            zaabyServer.UseZaabyServer<ITest>()
                .UseUrls("http://localhost:5002")
                .UseZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IAliceServices", new List<string> {"http://localhost:5001"}},
                    {"ICarolServices", new List<string> {"http://localhost:5003"}}
                })
                .Run();
        }
    }
}
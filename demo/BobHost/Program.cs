using System.Collections.Generic;
using Interfaces;
using Zaaby;

namespace BobHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = ZaabyServer.GetInstance();
            server.AddZaabyService<IService>()
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
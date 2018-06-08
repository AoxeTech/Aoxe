using System.Collections.Generic;
using Interfaces;
using Zaaby;
using Zaaby.Client;

namespace AppleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance()
                .UseZaabyServer<ITest>()
                .UseZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IBananaServices", new List<string> {"http://localhost:5001"}}
                })
                .UseUrls("http://localhost:5000").Run();
        }
    }
}
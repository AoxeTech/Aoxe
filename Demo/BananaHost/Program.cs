using System.Collections.Generic;
using Interfaces;
using Zaaby;
using Zaaby.Client;

namespace BananaHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance()
                .UseZaabyServer<ITest>()
                .UseZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IAppleServices", new List<string> {"http://localhost:5000"}}
                })
                .UseUrls("http://localhost:5001").Run();
        }
    }
}
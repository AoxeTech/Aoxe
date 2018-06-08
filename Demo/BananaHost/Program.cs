using Interfaces;
using Zaaby;

namespace BananaHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance()
                .UseZaabyServer<ITest>()
                .UseUrls("http://localhost:5001").Run();
        }
    }
}
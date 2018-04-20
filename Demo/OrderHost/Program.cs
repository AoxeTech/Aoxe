using Zaaby;

namespace OrderHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance()
                .UseZaabyRepository()
                .UseZaabyApplicationService()
                .UseUrls("http://*:5001")
                .Run();
        }
    }
}
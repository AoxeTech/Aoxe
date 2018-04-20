using Zaaby;

namespace ShippingHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance()
                .UseZaabyRepository()
                .UseZaabyApplicationService()
                .UseUrls("http://*:5002")
                .Run();
        }
    }
}
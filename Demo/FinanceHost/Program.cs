using Zaaby;

namespace FinanceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance()
                .UseZaabyRepository()
                .UseZaabyApplicationService()
                .UseUrls("http://*:5000")
                .Run();
        }
    }
}
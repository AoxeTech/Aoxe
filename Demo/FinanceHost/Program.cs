using Zaaby;

namespace FinanceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance().UseZaabyApplicationService().Run();
        }
    }
}
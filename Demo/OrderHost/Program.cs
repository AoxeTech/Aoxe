using System.Collections.Generic;
using Zaaby;

namespace OrderHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.GetInstance()
                .UseZaabyRepository()
                .UseZaabyApplicationService(
                new Dictionary<string, List<string>>
                {
                    {
                        "IFinanceApplication.ICustomerFinanceApplication",
                        new List<string> {"http://192.168.5.223:2500/"}
                    },
                    {
                        "IShippingApplication.IFreightApplication",
                        new List<string> {"http://192.168.5.223:2502/",}
                    }
                }).Run();
        }
    }
}
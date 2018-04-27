using IFinanceApplication.DTOs;
using Zaaby.Core;

namespace IFinanceApplication
{
    public interface ICustomerFinanceApplication : IZaabyApplicationService
    {
        bool Charge(CustomerChargeParam param);

        string FinanceSystemTest();
    }
}
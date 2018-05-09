using IFinanceApplication.DTOs;
using Zaaby.Core.Application;

namespace IFinanceApplication
{
    public interface ICustomerFinanceApplication : IApplicationService
    {
        bool Charge(CustomerChargeParam param);

        string FinanceSystemTest();
    }
}
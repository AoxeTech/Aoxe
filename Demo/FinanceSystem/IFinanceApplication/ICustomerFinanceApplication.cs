using IFinanceApplication.DTOs;

namespace IFinanceApplication
{
    public interface ICustomerFinanceApplication
    {
        bool Charge(CustomerChargeParam param);
    }
}
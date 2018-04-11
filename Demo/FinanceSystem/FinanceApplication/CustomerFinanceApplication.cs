using System;
using IFinanceApplication;
using IFinanceApplication.DTOs;

namespace FinanceApplication
{
    public class CustomerFinanceApplication:ICustomerFinanceApplication
    {
        public bool Charge(CustomerChargeParam param)
        {
            throw new NotImplementedException();
        }

        public Guid GetId()
        {
            return Guid.NewGuid();
        }
    }
}
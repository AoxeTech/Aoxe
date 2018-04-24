using System;
using IFinanceApplication;
using IFinanceApplication.DTOs;

namespace FinanceApplication
{
    public class CustomerFinanceApplication : ICustomerFinanceApplication
    {
        public CustomerFinanceApplication()
        {
            
        }
        
        public bool Charge(CustomerChargeParam param)
        {
            throw new NotImplementedException();
        }

        public int GetId()
        {
            return 1;
        }
    }
}
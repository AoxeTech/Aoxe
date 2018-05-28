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

        public string FinanceSystemTest()
        {
            return $"From CustomerFinanceApplication. {DateTimeOffset.Now.UtcTicks}";
        }

        public CustomerDto GetCustomer(Guid id)
        {
            //throw new Exception("haskdjfhlakjshdflkjahsdlfkjasdhfkjlasdhflkajshdfkashdfkjhas");
            return new CustomerDto { Id = id, Name = "apple", CreateTime = DateTimeOffset.Now };
        }
    }
}
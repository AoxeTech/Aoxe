using System;
using IShippingApplication;
using IShippingApplication.DTOs;

namespace ShippingApplication
{
    public class FreightApplication : IFreightApplication
    {
        public int FreightCharge(Cargo cargo)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}
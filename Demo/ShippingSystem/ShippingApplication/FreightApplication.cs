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

        public string ShippiingSystemTest()
        {
            return $"From FreightApplication. {DateTimeOffset.Now.UtcTicks}";
        }
    }
}
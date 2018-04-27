using IShippingApplication.DTOs;
using Zaaby.Core;

namespace IShippingApplication
{
    public interface IFreightApplication : IZaabyApplicationService
    {
        int FreightCharge(Cargo cargo);
        string ShippiingSystemTest();
    }
}
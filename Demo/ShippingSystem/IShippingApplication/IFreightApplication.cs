using IShippingApplication.DTOs;
using Zaaby.Core.Application;

namespace IShippingApplication
{
    public interface IFreightApplication : IApplicationService
    {
        int FreightCharge(Cargo cargo);
        string ShippiingSystemTest();
    }
}
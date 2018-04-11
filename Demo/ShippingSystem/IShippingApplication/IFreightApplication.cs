using IShippingApplication.DTOs;

namespace IShippingApplication
{
    public interface IFreightApplication
    {
        int FreightCharge(Cargo cargo);
    }
}
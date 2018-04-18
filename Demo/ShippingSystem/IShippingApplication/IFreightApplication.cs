using System;
using IShippingApplication.DTOs;
using Zaaby.Core;

namespace IShippingApplication
{
    public interface IFreightApplication : IZaabyAppService
    {
        int FreightCharge(Cargo cargo);
        Guid GetGuid();
    }
}
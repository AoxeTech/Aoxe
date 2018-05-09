using IOrderApplication.DTOs;
using Zaaby.Core.Application;

namespace IOrderApplication
{
    public interface IOrderParentApplication : IApplicationService
    {
        OrderParentDto GetOrderParentDto(string id);
        string OrderSystemTest();
    }
}
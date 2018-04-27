using IOrderApplication.DTOs;
using Zaaby.Core;

namespace IOrderApplication
{
    public interface IOrderParentApplication : IZaabyApplicationService
    {
        OrderParentDto GetOrderParentDto(string id);
        string OrderSystemTest();
    }
}
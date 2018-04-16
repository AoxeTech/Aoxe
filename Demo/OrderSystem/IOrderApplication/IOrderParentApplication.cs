using System;
using IOrderApplication.DTOs;
using Zaaby.Core;

namespace IOrderApplication
{
    public interface IOrderParentApplication : IZaabyAppService
    {
        OrderParentDto GetOrderParentDto(string id);
        int GetId();
    }
}
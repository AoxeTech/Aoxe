using System;
using IFinanceApplication;
using IOrderApplication;
using IOrderApplication.DTOs;
using IShippingApplication;
using OrderDomain.IRepository;

namespace OrderApplication
{
    public class OrderParentApplication : IOrderParentApplication
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerFinanceApplication _customerFinanceApplication;
        private readonly IFreightApplication _freightApplication;

        public OrderParentApplication(IOrderRepository orderParentRepository,
            ICustomerFinanceApplication customerFinanceApplication, IFreightApplication freightApplication)
        {
            _orderRepository = orderParentRepository;
            _customerFinanceApplication = customerFinanceApplication;
            _freightApplication = freightApplication;
        }

        public OrderParentDto Test()
        {
            throw new Exception("aksdjflaksdjf;lkajsd;fkjasd;lkfja;sdlkfj;alskdjf;lkasdjf;lsak");
            return new OrderParentDto
            {
                Id = "testId",
                CreateTime = DateTimeOffset.Now
            };
        }

        public OrderParentDto GetOrderParentDto(string id)
        {
            var order = _orderRepository.Get(id);
            return new OrderParentDto
            {
                Id = order.Id,
                CreateTime = order.CreateTime
            };
        }

        public string OrderSystemTest()
        {
            return $"{_customerFinanceApplication.FinanceSystemTest()}\r\n" +
                   $"From OrderParentApplication. {DateTimeOffset.Now.UtcTicks}";
        }
    }
}
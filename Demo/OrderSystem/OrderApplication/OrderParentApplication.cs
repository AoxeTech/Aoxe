using IFinanceApplication;
using IFinanceApplication.DTOs;
using IOrderApplication;
using IOrderApplication.DTOs;
using IShippingApplication;
using IShippingApplication.DTOs;
using OrderDomain.IRepository;

namespace OrderApplication
{
    public class OrderParentApplication : IOrderParentApplication
    {
        private readonly IOrderParentRepository _orderParentRepository;
        private readonly IToBeCheckedOrderRepository _toBeCheckedOrderRepository;
        private readonly ICustomerFinanceApplication _customerFinanceApplication;
        private readonly IFreightApplication _freightApplication;

        public OrderParentApplication(IOrderParentRepository orderParentRepository,
            IToBeCheckedOrderRepository toBeCheckedOrderRepository,
            ICustomerFinanceApplication customerFinanceApplication, IFreightApplication freightApplication)
        {
            _orderParentRepository = orderParentRepository;
            _toBeCheckedOrderRepository = toBeCheckedOrderRepository;
            _customerFinanceApplication = customerFinanceApplication;
            _freightApplication = freightApplication;
        }

        public void CommitToBeCheckedOrder(string id)
        {
            var tbc = _toBeCheckedOrderRepository.Get(id);
            var freight = _freightApplication.FreightCharge(new Cargo
            {
                Height = tbc.Height,
                Lenght = tbc.Lenght,
                Weight = tbc.Weight,
                Width = tbc.Width
            });
            _customerFinanceApplication.Charge(new CustomerChargeParam
            {
                CustomerId = tbc.UserId,
                FeeByCent = freight
            });
            var orderParent = tbc.Commit();
            _toBeCheckedOrderRepository.Modify(tbc);
            _orderParentRepository.Add(orderParent);
        }

        public OrderParentDto GetOrderParentDto(string id)
        {
            var order = _orderParentRepository.Get(id);
            return new OrderParentDto
            {
                Id = order.Id,
                CreateTimeOffset = order.CreateTimeOffset
            };
        }

        public int GetId()
        {
            return _customerFinanceApplication.GetId();
        }
    }
}
using System;
using System.Collections.Generic;
using OrderDomain.IRepository;
using Zaaby.Core;

namespace OrderDomain.DomainServices
{
    public class OrderService : IZaabyDomainService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public OrderService(IOrderRepository orderRepository, ICurrencyRepository currencyRepository)
        {
            _orderRepository = orderRepository;
            _currencyRepository = currencyRepository;
        }

        public void ModifyOrderCurrency(List<string> orderIds, string currencyType)
        {
            var currency = _currencyRepository.Get(currencyType);
            if (currency == null)
                throw new ArgumentException("The curency is not exist.");
            if (!currency.IsValid)
                throw new ArgumentException("The curency is not valid.");
            
            var orders = _orderRepository.Get(orderIds);
            orders.ForEach(order => order.ModifyCurrency(currency.Id));
            _orderRepository.Modify(orders);
        }
    }
}
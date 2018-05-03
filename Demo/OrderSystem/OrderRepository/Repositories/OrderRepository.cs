using System;
using System.Collections.Generic;
using System.Linq;
using OrderDomain.AggregateRoots;
using OrderDomain.IRepository;
using OrderRepository.PersistentObjects;
using Zaabee.Mongo.Core;

namespace OrderRepository.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoDbRepository _mongoDbRepository;

        public OrderRepository(IMongoDbRepository mongoDbRepository)
        {
            _mongoDbRepository = mongoDbRepository;
        }

        public void Add(Order order)  
        {
            _mongoDbRepository.Add(Convert(order));
        }

        public void Add(List<Order> orders)
        {
            _mongoDbRepository.AddRange(Convert(orders));
        }

        public bool Delete(Order order)
        {
            return _mongoDbRepository.Delete(Convert(order)) > 0;
        }

        public int Delete(List<Order> orders)
        {
            var ids = orders.Select(p => p.Id).ToList();
            return (int) _mongoDbRepository.Delete<OrderParentPo>(p => ids.Contains(p.Id));
        }

        public bool Modify(Order orderParent)
        {
            return _mongoDbRepository.Update(Convert(orderParent)) > 0;
        }

        public int Modify(List<Order> orderParents)
        {
            var pos = Convert(orderParents);
            var result = 0;
            pos.ForEach(po => result += (int) _mongoDbRepository.Update(po));
            return result;
        }

        public Order Get(string id)
        {
            return Convert(_mongoDbRepository.GetQueryable<OrderParentPo>().FirstOrDefault(p => p.Id == id));
        }

        public List<Order> Get(List<string> ids)
        {
            return Convert(_mongoDbRepository.GetQueryable<OrderParentPo>().Where(p=>ids.Contains(p.Id)).ToList());
        }

        public List<Order> GetAll()
        {
            return Convert(_mongoDbRepository.GetQueryable<OrderParentPo>().ToList());
        }

        public List<Order> GetValidOrders()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrdersByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        #region Convert

        private static List<OrderParentPo> Convert(List<Order> orders)
        {
            return new List<OrderParentPo>();
        }

        private static OrderParentPo Convert(Order order)
        {
            return new OrderParentPo { };
        }

        private static List<Order> Convert(List<OrderParentPo> orders)
        {
            return new List<Order>();
        }

        private static Order Convert(OrderParentPo order)
        {
            return null;
        }

        #endregion
    }
}
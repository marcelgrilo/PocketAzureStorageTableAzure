using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AzureStorageTableHelper.Interfaces;

using Logger.Domain.Entities;
using Logger.Domain.Services.Interfaces;

namespace Logger.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAzureTableStorage<Order> orderRepository;
        public OrderService(IAzureTableStorage<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task AddOrder(Order item)
        {
            await this.orderRepository.Insert(item);
        }

        public async Task DeleteOrder(DateTime date, Guid businessId, string orderId)
        {
            await this.orderRepository.Delete(Order.GeneratePartitionKey(businessId, orderId), Order.GenerateRowKey(date));
        }

        public async Task<Order> GetOrder(DateTime date, Guid businessId, string orderId)
        {
            return await this.orderRepository.GetItem(Order.GeneratePartitionKey(businessId, orderId), Order.GenerateRowKey(date));
        }

        public async Task<List<Order>> GetOrders()
        {
            return await this.orderRepository.GetList();
        }

        public async Task<bool> OrderExists(DateTime date, Guid businessId, string orderId)
        {
            return await this.orderRepository.GetItem(Order.GeneratePartitionKey(businessId, orderId), Order.GenerateRowKey(date)) != null;
        }

        public async Task UpdateOrder(Order item)
        {
            await this.orderRepository.Update(item);
        }
    }
}

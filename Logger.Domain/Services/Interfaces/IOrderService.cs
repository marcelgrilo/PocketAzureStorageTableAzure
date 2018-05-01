using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Logger.Domain.Entities;

namespace Logger.Domain.Services.Interfaces
{
    public interface IOrderService
    {
        Task AddOrder(Order item);
        Task DeleteOrder(DateTime date, Guid businessId, string orderId);
        Task<Order> GetOrder(DateTime date, Guid businessId, string orderId);
        Task<List<Order>> GetOrders();
        Task<bool> OrderExists(DateTime date, Guid businessId, string orderId);
        Task UpdateOrder(Order item);
    }
}

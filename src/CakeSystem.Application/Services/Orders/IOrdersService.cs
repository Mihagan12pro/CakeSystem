using CakeSystem.Application.DTOs.Orders;
using CakeSystem.Domain.Entities.Order;

namespace CakeSystem.Application.Services.Orders
{
    public interface IOrdersService
    {
        Task<string> CreateAsync(
            NewOrderDto order,
            string login,
            CancellationToken token);

        Task<bool> CheckClientAsync(
            string login,
            string orderId,
            CancellationToken token);

        Task<IEnumerable<Order>> GetOrdersAsync(
            Filters<Order> filters,
            CancellationToken token);

        Task<OrderHistoryRow> GetStatusAsync(
            string orderId, 
            CancellationToken token);
    }
}

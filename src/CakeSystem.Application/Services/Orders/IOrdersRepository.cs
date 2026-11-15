using CakeSystem.Application.DTOs.Orders;
using CakeSystem.Domain.Entities.Order;

namespace CakeSystem.Application.Services.Orders
{
    public interface IOrdersRepository
    {
        Task<string> AddAsync(
            NewOrderDto orderDto, 
            string login, 
            CancellationToken token);

        Task<Order> GetByIdAsync(
            string id,
            CancellationToken token);

        Task<IEnumerable<Order>> GetOrdersAsync(
            Filters<Order> filters,
            CancellationToken token);

        Task<OrderHistoryRow> GetLastChangesAsync(
            string id,
            CancellationToken token);
    }
}

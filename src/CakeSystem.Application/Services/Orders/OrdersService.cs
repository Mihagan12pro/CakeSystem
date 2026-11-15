using CakeSystem.Application.DTOs.Orders;
using CakeSystem.Domain.Entities.Order;

namespace CakeSystem.Application.Services.Orders
{
    internal class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public async Task<string> CreateAsync(
            NewOrderDto order,
            string login,
            CancellationToken token)
                => await _ordersRepository.AddAsync(order, login, token);

        public async Task<bool> CheckClientAsync(
            string login,
            string orderId,
            CancellationToken token)
                => (await _ordersRepository.GetByIdAsync(orderId, token)).ClientLogin == login;

        public async Task<OrderHistoryRow> GetStatusAsync(
            string orderId,
            CancellationToken token)
                => await _ordersRepository.GetLastChangesAsync(orderId, token);

        public async Task<IEnumerable<Order>> GetOrdersAsync(
            Filters<Order> filters,
            CancellationToken token)
                => await _ordersRepository.GetOrdersAsync(filters, token);

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;           
        }
    }
}

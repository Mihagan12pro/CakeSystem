using CakeSystem.Application.DTOs.Orders;

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

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;           
        }
    }
}

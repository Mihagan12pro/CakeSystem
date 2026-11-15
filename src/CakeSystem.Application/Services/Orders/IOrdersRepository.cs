using CakeSystem.Application.DTOs.Orders;

namespace CakeSystem.Application.Services.Orders
{
    public interface IOrdersRepository
    {
        Task<string> AddAsync(
            NewOrderDto orderDto, 
            string login, 
            CancellationToken token);
    }
}

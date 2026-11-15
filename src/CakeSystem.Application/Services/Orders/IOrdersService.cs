using CakeSystem.Application.DTOs.Orders;

namespace CakeSystem.Application.Services.Orders
{
    public interface IOrdersService
    {
        Task<string> CreateAsync(
            NewOrderDto order,
            string login,
            CancellationToken token);
    }
}

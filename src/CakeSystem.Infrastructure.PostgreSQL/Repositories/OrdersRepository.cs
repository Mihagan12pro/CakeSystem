using CakeSystem.Application;
using CakeSystem.Application.DTOs.Orders;
using CakeSystem.Application.Services.Orders;
using CakeSystem.Domain.Entities;
using CakeSystem.Domain.Entities.Order;
using CakeSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CakeSystem.Infrastructure.PostgreSQL.Repositories
{
    internal class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _dbContext;

        public async Task<string> AddAsync(
            NewOrderDto orderDto, 
            string login, 
            CancellationToken token)
        {
            User user = await _dbContext.Users.FirstAsync(
                u => u.Login == login, 
                token
            );

            StringBuilder id = new StringBuilder();

            DateTime dateTime = DateTime.UtcNow;
            
            DateOnly now = new DateOnly(
                dateTime.Year, 
                dateTime.Month,
                dateTime.Day
            );

            id.Append(now.Day);
            id.Append(now.Month);
            id.Append(now.Year);

            if (user.FullName != null)
            {
                string[] fullName = user.FullName.Split(' ');

                id.Append(char.ToUpper(fullName[0][0]));

                if (fullName.Length > 1)
                    id.Append(char.ToUpper(fullName[1][0]));
                else
                    id.Append('_');
            }
            else
                id.Append("__");

            id.Append(
                await _dbContext.Orders.CountAsync(o => o.StartDate == now) + 1
            );

            Order order = new Order
            {
                Id = id.ToString(),

                Description = orderDto.Description,

                ClientLogin = login,

                StartDate = now,

                Status = OrderStatus.New
            };
            OrderHistoryRow historyRow = new OrderHistoryRow
            {
                ChangedAt = dateTime,

                Status = order.Status,

                OrderId = order.Id
            };

            await _dbContext.Orders.AddAsync(order, token);
            await _dbContext.OrdersStatusHistory.AddAsync(historyRow, token);

            await _dbContext.SaveChangesAsync(token);

            return order.Id;
        }

        public async Task<Order> GetByIdAsync(
            string id,
            CancellationToken token)
                => await _dbContext.Orders.FirstAsync(o => o.Id == id, token);

        public async Task<OrderHistoryRow> GetLastChangesAsync(
            string orderId,
            CancellationToken token)
                => await _dbContext.OrdersStatusHistory
                    .Where(oh => oh.OrderId == orderId)         
                    .OrderByDescending(oh => oh.ChangedAt)
                    .FirstAsync(token);


        public async Task<IEnumerable<Order>> GetOrdersAsync(
            Filters<Order> filters,
            CancellationToken token)
        {
            IQueryable<Order> orders = _dbContext.Orders;
            foreach (var filter in filters)
                orders = orders.Where(filter);

            return orders;
        }

        public OrdersRepository(AppDbContext dbContext)
            => _dbContext = dbContext;
    }
}

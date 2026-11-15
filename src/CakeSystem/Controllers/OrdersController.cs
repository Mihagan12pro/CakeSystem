using CakeSystem.Application;
using CakeSystem.Application.DTOs.Orders;
using CakeSystem.Application.Services.Orders;
using CakeSystem.Domain.Entities.Order;
using CakeSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CakeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
       [HttpPost()]
       [Authorize(Roles = "Client")]
        public async Task<IActionResult> MakeOrder(
           [FromBody] NewOrderDto order,
           [FromServices] IOrdersService ordersService,
           CancellationToken token)
       {
            string login = User.FindFirstValue("sub");
            var id = await ordersService.CreateAsync(
                order, 
                login, 
                token
            );

            return Created($"{Request.Scheme}://{Request.Host}/api/orders/{id}", id);
       }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> CheckOrderHistory(
            [FromRoute] string id,
            [FromServices] IOrdersService ordersService,
            CancellationToken token)
        {
            Roles role = (Roles)Enum.Parse(
                typeof(Roles),
                User.FindFirstValue("role")
            );

            if (role == Roles.Client)
            {
                if (!await ordersService.CheckClientAsync(User.FindFirstValue("sub"), id, token))
                    return StatusCode(StatusCodes.Status403Forbidden); 
            }

            return Ok(await ordersService.GetStatusAsync(id, token));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(
            [FromQuery] OrderStatus? status,
            [FromServices] IOrdersService ordersService,
            CancellationToken token)
        {
            var filters = new Filters<Order>();

            Roles role = (Roles)Enum.Parse(
                typeof(Roles),
                User.FindFirstValue("role")
            );

            string login = User.FindFirstValue("sub");

            if (role == Roles.Client)
            {
                filters.Add(o => o.ClientLogin == login);
            }
            else if (role == Roles.AccountManager)
            {
                filters.Add(o => o.ManagerLogin == login || o.ManagerLogin == null);
            }

            if (status != null)
                filters.Add(o => o.Status == status);

            var orders = await ordersService.GetOrdersAsync(filters, token);

            return Ok(orders.Select(o => new GetOrderDto(o.Id, o.Description)));
        }
    }
}

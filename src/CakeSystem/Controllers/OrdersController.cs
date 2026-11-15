using CakeSystem.Application.DTOs.Orders;
using CakeSystem.Application.Security.Jwt;
using CakeSystem.Application.Services.Orders;
using CakeSystem.Domain.Entities;
using CakeSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;

namespace CakeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
       [Authorize(Roles = "Client")]
       [HttpPost()]
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

            return Created($"{Request.Scheme}://{Request.Host}/{id}", login);
       }
    }
}

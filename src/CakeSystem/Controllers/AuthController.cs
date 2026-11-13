using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Application.Services.Auth;
using CakeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CakeSystem.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(
            [FromForm] NewUserDto user,
            [FromServices] IAuthService service,
            CancellationToken token)
        {
            if (await service.AddAsync(user, token))
                return Redirect("/Auth/SignIn");

            return View("Error", new ErrorViewModel()
            {
                 StatusCode = HttpStatusCode.Conflict,

                 ErrorMessage = "Пользователь с данным логином уже зарегистрирован!"
            });
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
    }
}

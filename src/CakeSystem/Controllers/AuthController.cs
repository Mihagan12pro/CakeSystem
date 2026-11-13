using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Application.Services.Auth;
using CakeSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;

namespace CakeSystem.Controllers
{
    public class AuthController : Controller
    {
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

        [HttpPost]
        public async Task<IActionResult> SignIn(
            [FromServices] IAuthService service,
            [FromServices] IOptions<CookiesSettings> cookiesSettings,
            [FromForm] LoginUserDto userDto,
            CancellationToken token)
        {
            GetUserDto user = await service.GetUser(userDto, token);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
        
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                return Redirect("/Home/Index");
            }

            return View("Error", new ErrorViewModel()
            {
                StatusCode = HttpStatusCode.BadRequest,

                ErrorMessage = "Неверный логин или пароль!"
            });
        }

        [HttpGet]
        public IActionResult SignUp()
            => View();

        [HttpGet]
        public IActionResult SignIn()
            => View();
    }
}

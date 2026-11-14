using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Application.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CakeSystem.Controllers
{
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SignUp(
            [FromForm] NewUserDto user,
            [FromServices] IAuthService service,
            CancellationToken token)
        {
            return Ok();
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

            return Ok();
        }
    }
}

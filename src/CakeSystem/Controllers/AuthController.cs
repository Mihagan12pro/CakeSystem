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
            [FromBody] NewUserDto user,
            [FromServices] IAuthService service,
            CancellationToken token)
        {
            if (await service.AddAsync(user, token))
                return Created();

            return Conflict($"The user with Login = {user.Login} is already exists!");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(
            [FromServices] IAuthService service,
            [FromServices] IOptions<CookiesSettings> cookiesSettings,
            [FromBody] LoginUserDto userDto,
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

using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Application.Security.Jwt;
using CakeSystem.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CakeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("sigup")]

        public async Task<IActionResult> SignUp(
            [FromBody] NewUserDto user,
            [FromServices] IAuthService service,
            CancellationToken token)
        {
            if (await service.AddAsync(user, token))
                return Created();

            return Conflict($"The user with Login = {user.Login} is already exists!");
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(
            [FromServices] IAuthService authService,
            [FromServices] IJwtTokenService jwtTokenService,
            [FromBody] LoginUserDto userDto,
            CancellationToken token)
        {
            GetUserDto user = await authService.GetUser(userDto, token);

            if (user != null)
            {
                string jwt = jwtTokenService.Create(user);

                return Ok(jwt);
            }

            return BadRequest("Bad login or password!");
        }
    }
}

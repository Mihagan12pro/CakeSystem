using CakeSystem.Application.DTOs.Auth;

namespace CakeSystem.Application.Security.Jwt
{
    public interface IJwtTokenService
    {
        string Create(GetUserDto user);

        string Extract(string claim);
    }
}

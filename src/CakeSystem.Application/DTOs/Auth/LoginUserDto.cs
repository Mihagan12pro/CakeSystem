using CakeSystem.Domain.Enums;

namespace CakeSystem.Application.DTOs.Auth
{
    public record LoginUserDto(
        string Login,
        string Password);

    public record GetUserDto(
        string Login, 
        string Fullname,
        Roles Role);
}

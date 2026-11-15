using CakeSystem.Domain.Enums;

namespace CakeSystem.Application.DTOs.Auth
{
    public record GetUserDto(
        string Login, 
        string Fullname,
        Roles Role);
}

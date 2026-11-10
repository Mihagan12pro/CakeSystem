using System.ComponentModel.DataAnnotations;

namespace CakeSystem.Application.DTOs.Auth
{
    public record class PutUserDto(
        [Required] string Login,
        string? Password,
        string? Role,
        string? FullName);
}

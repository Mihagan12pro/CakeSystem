using System.ComponentModel.DataAnnotations;

namespace CakeSystem.Application.DTOs.Auth
{
    public record class NewUserDto(
        [Required] string Login, 
        [Required] string Password, 
        string? FullName);
}

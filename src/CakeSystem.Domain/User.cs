using CakeSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CakeSystem.Domain
{
    public class User
    {
        [Key]
        public required string Login { get; set; }

        public required string Password { get; set; }

        public required Roles Role { get; set; }

        public string? FullName { get; set; }
    }
}

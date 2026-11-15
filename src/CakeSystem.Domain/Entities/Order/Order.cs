using CakeSystem.Domain.Enums;
using System.Text.Json.Serialization;

namespace CakeSystem.Domain.Entities.Order
{
    public class Order
    {
        public required string Id { get; set; }

        public required string Description { get; set; }

        public required DateOnly StartDate { get; set; }

        public required string ClientLogin { get; set; }

        public DateOnly? EndDate { get; set; }

        public decimal? Price { get; set; }

        public string? ManagerLogin { get; set; }
        
        public required OrderStatus Status { get; set; }

        [JsonIgnore]
        public User Manager { get;set; } = null!;

        [JsonIgnore]
        public User Client { get; set; } = null!;
    }
}

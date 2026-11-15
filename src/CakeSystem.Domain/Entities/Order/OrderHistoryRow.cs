using CakeSystem.Domain.Enums;
using System.Text.Json.Serialization;

namespace CakeSystem.Domain.Entities.Order
{
    public class OrderHistoryRow
    {
        public Guid Id { get; set; }

        public required string OrderId { get; set; }

        public required DateTime ChangedAt { get; set; }

        public required OrderStatus Status { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
    }
}

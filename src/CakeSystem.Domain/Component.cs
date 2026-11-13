namespace CakeSystem.Domain
{
    public class Component
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public required string Type { get; set; }

        public decimal Price { get; set; }

        public required string Title { get; set; }

        public required string UnitMeasurement { get; set; }

        public required string PrePacking { get; set; }

        public string? Gost { get; set; }

        public string? Characteristics { get; set; }
    }
}

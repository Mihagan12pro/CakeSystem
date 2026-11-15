using System.ComponentModel.DataAnnotations;

namespace CakeSystem.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MinLength(3)]
        public required string Name { get; set; }

        public required bool IsPrepack {  get; set; }
    }
}

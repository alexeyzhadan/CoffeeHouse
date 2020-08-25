using System.ComponentModel.DataAnnotations;

namespace CoffeeHouse.Models
{
    public class OrderProd
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Mark { get; set; }

        [Required]
        public int Count { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
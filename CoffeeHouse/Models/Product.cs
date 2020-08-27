using CoffeeHouse.Services.DbRepositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CoffeeHouse.Models
{
    public class Product : IDefaultEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
using CoffeeHouse.Services.DbRepositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeHouse.Models
{
    public class Client : IDefaultEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public decimal Cashback { get; set; } = 0.0m;

        public List<Order> Orders { get; set; }
    }
}
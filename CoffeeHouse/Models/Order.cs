using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeHouse.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        public int? ClientId { get; set; }

        [Required]
        public int CashierId { get; set; }

        public Client Client { get; set; }
        public Cashier Cashier { get; set; }
        public List<OrderProd> OrderProds { get; set; }
    }
}
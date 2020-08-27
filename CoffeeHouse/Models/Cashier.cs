﻿using CoffeeHouse.Services.DbRepositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CoffeeHouse.Models
{
    public class Cashier : IDefaultEntity
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
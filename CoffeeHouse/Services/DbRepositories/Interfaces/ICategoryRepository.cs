﻿using CoffeeHouse.Models;
using System.Linq;

namespace CoffeeHouse.Services.DbRepositories.Interfaces
{
    public interface ICategoryRepository : IDefaultEntityRepository<Category>
    {
        IQueryable<Category> GetAllOrderedByName();
    }
}
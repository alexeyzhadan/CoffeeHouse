﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHouse.Controllers
{
    public class OrderRegistration : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
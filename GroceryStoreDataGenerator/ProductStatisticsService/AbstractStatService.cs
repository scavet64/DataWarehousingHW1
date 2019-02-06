﻿using System;
using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public abstract class AbstractStatService : IProductStatService
    {
        protected readonly Random rng = new Random();

        public Inventory Inventory { protected get; set; }

        protected int GetPercentage()
        {
            return rng.Next(100) + 1;
        }

        public abstract List<Product> GetProductsBasedOnStats();
    }
}

using System;
using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public abstract class AbstractStatService : IProductStatService
    {
        protected readonly Random Rng = new Random();
        public Inventory Inventory { protected get; set; }
        public abstract List<Product> GetProductsBasedOnStats(int itemsLeft);

        protected int GetPercentage()
        {
            return Rng.Next(100) + 1;
        }
    }
}
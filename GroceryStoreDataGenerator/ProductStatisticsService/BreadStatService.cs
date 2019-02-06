using System;
using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class BreadStatService : AbstractStatService
    {
        public const string BreadType = "Bread";
        private const int breadPercent = 50;

        public BreadStatService()
        {
        }

        public override List<Product> GetProductsBasedOnStats()
        {
            List<Product> products = new List<Product>();

            //got milk?
            if (GetPercentage() <= breadPercent)
            {
                products.Add(Inventory.Instance.GetRandomProductByType(BreadType));
            }

            return products;
        }
    }
}

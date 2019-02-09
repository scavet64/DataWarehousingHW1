using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class BreadStatService : AbstractStatService
    {
        public const string BreadType = "Bread";
        private const int BreadPercent = 50;

        public override List<Product> GetProductsBasedOnStats()
        {
            var products = new List<Product>();

            //got milk?
            if (GetPercentage() <= BreadPercent)
            {
                products.Add(Inventory.GetRandomProductByType(BreadType));
            }

            return products;
        }
    }
}
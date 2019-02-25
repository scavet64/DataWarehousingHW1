using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class BreadStatService : AbstractStatService
    {
        public const string BreadType = "Bread";
        private const int BreadPercent = 50;

        public override List<Product> GetProductsBasedOnStats(int itemsLeft)
        {
            var products = new List<Product>();

            if (GetPercentage() <= BreadPercent)
            {
                products.Add(Inventory.PurchaseRandomProductByType(BreadType));
            }

            return products;
        }
    }
}
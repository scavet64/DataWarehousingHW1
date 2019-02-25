using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class BabyFoodAndDiaperStatService : AbstractStatService
    {
        public const string BabyFoodType = "Baby Food";
        private const int BabyFoodPercent = 20;
        public const string DiaperType = "Diapers";
        private const int DiaperPercent = 80;
        private const int DiapersWithoutBabyFoodPercent = 1;

        public override List<Product> GetProductsBasedOnStats(int itemsLeft)
        {
            var products = new List<Product>();

            if (GetPercentage() <= BabyFoodPercent)
            {
                products.Add(Inventory.PurchaseRandomProductByType(BabyFoodType));
                itemsLeft--;

                if (GetPercentage() <= DiaperPercent && itemsLeft > 0)
                {
                    products.Add(Inventory.PurchaseRandomProductByType(DiaperType));
                }
            }
            else
            {
                if (GetPercentage() <= DiapersWithoutBabyFoodPercent)
                {
                    products.Add(Inventory.PurchaseRandomProductByType(DiaperType));
                }
            }

            return products;
        }
    }
}
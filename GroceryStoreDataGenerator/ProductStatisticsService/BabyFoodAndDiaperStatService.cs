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

        public override List<Product> GetProductsBasedOnStats()
        {
            var products = new List<Product>();

            //got milk?
            if (GetPercentage() <= BabyFoodPercent)
            {
                products.Add(Inventory.GetRandomProductByType(BabyFoodType));
                
                //got cereal?
                if (GetPercentage() <= DiaperPercent)
                {
                    products.Add(Inventory.GetRandomProductByType(DiaperType));
                }
            }
            else
            {
                //cereal without milk
                if (GetPercentage() <= DiapersWithoutBabyFoodPercent)
                {
                    products.Add(Inventory.GetRandomProductByType(DiaperType));
                }
            }

            return products;
        }
    }
}
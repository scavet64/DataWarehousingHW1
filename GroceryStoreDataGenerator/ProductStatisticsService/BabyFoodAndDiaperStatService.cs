using System;
using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class BabyFoodAndDiaperStatService : AbstractStatService
    {
        public const string babyFoodType = "Baby Food";
        private const int babyfoodPercent = 20;
        public const string DiaperType = "Diapers";
        private const int diaperPercent = 80;
        private const int diapersWithoutBabyFoodPercent = 1;

        public BabyFoodAndDiaperStatService()
        {
        }

        public override List<Product> GetProductsBasedOnStats()
        {
            List<Product> products = new List<Product>();

            //got milk?
            if (GetPercentage() <= babyfoodPercent)
            {
                products.Add(Inventory.Instance.GetRandomProductByType(babyFoodType));
                //got cereal?
                if (GetPercentage() <= diaperPercent)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(DiaperType));
                }
            }
            else
            {
                //cereal without milk
                if (GetPercentage() <= diapersWithoutBabyFoodPercent)
                {
                    products.Add(Inventory.Instance.GetRandomProductByType(DiaperType));
                }
            }

            return products;
        }
    }
}

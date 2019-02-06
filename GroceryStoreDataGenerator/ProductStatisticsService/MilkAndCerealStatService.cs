using System;
using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class MilkAndCerealStatService : AbstractStatService
    {
        public const string MilkType = "Milk";
        private const int milkPercent = 70;
        public const string CerealType = "Cereal";
        private const int cerealPercent = 50;
        private const int cerealWithoutMilk = 5;

        public MilkAndCerealStatService()
        {

        }

        public override List<Product> GetProductsBasedOnStats()
        {
            List<Product> products = new List<Product>();

            //got milk?
            if (GetPercentage() <= milkPercent)
            {
                products.Add(Inventory.GetRandomProductByType(MilkType));
                //got cereal?
                if (GetPercentage() <= cerealPercent)
                {
                    products.Add(Inventory.GetRandomProductByType(CerealType));
                }
            }
            else
            {
                //cereal without milk
                if (GetPercentage() <= cerealWithoutMilk)
                {
                    products.Add(Inventory.GetRandomProductByType(CerealType));
                }
            }

            return products;
        }
    }
}

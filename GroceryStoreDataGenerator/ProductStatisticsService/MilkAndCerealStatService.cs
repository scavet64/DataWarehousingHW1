using System.Collections.Generic;
using GroceryStoreDataGenerator.Models;

namespace GroceryStoreDataGenerator.ProductStatisticsService
{
    public class MilkAndCerealStatService : AbstractStatService
    {
        public const string MilkType = "Milk";
        private const int MilkPercent = 70;
        public const string CerealType = "Cereal";
        private const int CerealPercent = 50;
        private const int CerealWithoutMilk = 5;

        public override List<Product> GetProductsBasedOnStats()
        {
            var products = new List<Product>();

            //got milk?
            if (GetPercentage() <= MilkPercent)
            {
                products.Add(Inventory.GetRandomProductByType(MilkType));
                
                //got cereal?
                if (GetPercentage() <= CerealPercent)
                {
                    products.Add(Inventory.GetRandomProductByType(CerealType));
                }
            }
            else
            {
                //cereal without milk
                if (GetPercentage() <= CerealWithoutMilk)
                {
                    products.Add(Inventory.GetRandomProductByType(CerealType));
                }
            }

            return products;
        }
    }
}